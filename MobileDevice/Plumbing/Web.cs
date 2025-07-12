using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.Screens;
using RestSharp;

namespace Pro4Soft.MobileDevice.Plumbing
{
    public class Web
    {
        public string AuthenticationToken { get; set; }

        internal static string ServerHost
        {
            get
            {
#if DEBUG
                return "http://app.rodionpronin.com:2020";
#else
                return "https://app.p4warehouse.com";
#endif
            }
        }

        private string _baseUrl;
        public string RawBaseUrl
        {
            get
            {
                var file = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "baseUrl.txt");
                if (!File.Exists(file))
                    return ServerHost;
                return Utils.ReadTextFile(Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "baseUrl.txt")).Trim();
            }
            set
            {
                var file = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "baseUrl.txt");
                if (string.IsNullOrWhiteSpace(value))
                {
                    if (File.Exists(file))
                        File.Delete(file);
                }
                else
                {
                    try { var _ = new Uri(value); }
                    catch { throw new BusinessWebException("Invalid url format"); }

                    if (!value.ToLower().Contains("//app."))
                        throw new BusinessWebException("Invalid url, must be of format: https://app.domain.com");
                    if (string.IsNullOrWhiteSpace(Singleton<Context>.Instance.Tenant))
                        throw new BusinessWebException("Tenant is missing");
                    Utils.WriteTextFile(file, value);
                    _baseUrl = value.Replace("//app.", $"//{Singleton<Context>.Instance.Tenant}.");
                }
                _client = new RestClient(BaseUrl);
            }
        }

        public string BaseUrl
        {
            get
            {
                if (_baseUrl != null)
                    return _baseUrl;

                _baseUrl = RawBaseUrl.Replace("//app.", $"//{Singleton<Context>.Instance.Tenant}.");
                return _baseUrl;
            }
        }

        private RestClient _client;
        private RestClient Client => _client ??= new RestClient(BaseUrl);

        public void ResetWeb()
        {
            _client = null;
        }

        public async Task<T> GetInvokeAsync<T>(string url, bool isSilentMode = false, bool redirectOn401 = true) where T : class
        {
            return Utils.DeserializeFromJson<T>(await GetInvokeAsync(url, isSilentMode, redirectOn401), url.Trim('/').StartsWith("odata") ? "value" : null);
        }
        
        public async Task<string> GetInvokeAsync(string url, bool isSilentMode = false, bool redirectOn401 = true)
        {
            var result = await WebInvokeAsync(url, Method.Get, null, isSilentMode, redirectOn401);
            if (!result.IsSuccessful)
                throw new BusinessWebException(result.StatusCode, string.IsNullOrWhiteSpace(result.Content) ? result.ErrorMessage : result.Content);
            return result.Content;
        }

        public async Task<T> PostInvokeAsync<T>(string url, object payload, bool isSilentMode = false, bool redirectOn401 = true) where T : class
        {
            return Utils.DeserializeFromJson<T>(await PostInvokeAsync(url, payload, isSilentMode, redirectOn401));
        }

        public async Task<string> PostInvokeAsync(string url, object payload, bool isSilentMode = false, bool redirectOn401 = true)
        {
            var result = await WebInvokeAsync(url, Method.Post, payload, isSilentMode, redirectOn401);
            if (!result.IsSuccessful)
                throw new BusinessWebException(result.StatusCode, string.IsNullOrWhiteSpace(result.Content) ? result.ErrorMessage : result.Content);
            return result.Content;
        }

        public async Task<RestResponse> WebInvokeAsync(string url, Method method = Method.Get, object payload = null, bool isSilentMode = false, bool redirectOn401 = true)
        {
            try
            {
                var split = url.Split('?');
                if (split.Length > 1)
                    url = split[0];
                var request = new RestRequest(url, method) {RequestFormat = DataFormat.Json};
                if (split.Length > 1)
                {
                    var qryParsString = split.Last();
                    var parsed = HttpUtility.ParseQueryString(qryParsString);
                    foreach (var par in parsed.Cast<string>().Where(c => !string.IsNullOrWhiteSpace(c)))
                        request.AddQueryParameter(par, parsed.Get(par));
                }

                if (!string.IsNullOrWhiteSpace(AuthenticationToken))
                    request.AddHeader("AuthenticationToken", AuthenticationToken);
                if (method == Method.Post || method == Method.Patch)
                {
                    var json = Utils.SerializeToStringJson(payload);
                    request.AddStringBody(json, "application/json");
                }

                if(!isSilentMode)
                    Main.SetLoading();

                var result = await Client.ExecuteAsync(request);
                if (!result.IsSuccessful && result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (redirectOn401)
                    {
                        AuthenticationToken = null;
                        await Singleton<Context>.Instance.Logout();
                        await Main.NavigateToView<LoginView>(s => { s.SetError(!string.IsNullOrWhiteSpace(result.Content) ? result.Content : "Session logged out"); });
                    }
                    else
                        throw new Exception(string.IsNullOrWhiteSpace(result.Content) ? result.ErrorMessage : result.Content);
                }
                return result;
            }
            finally
            {
                if (!isSilentMode)
                    Main.SetFinishedLoading();
            }
        }

        public async Task<RestResponse> UploadStream(string url, Stream source, bool isSilentMode = false, string fileName = null)
        {
            try
            {
                var request = new RestRequest(url, Method.Post);
                if (!string.IsNullOrWhiteSpace(AuthenticationToken))
                    request.AddHeader("AuthenticationToken", AuthenticationToken);
                request.AddFile("file", () => source, fileName??"photo.png", "image/png");
                if (!isSilentMode)
                    Main.SetLoading();

                var result = await Client.ExecuteAsync(request);
                if (!result.IsSuccessful)
                    throw new BusinessWebException(result.StatusCode, result.Content);
                return result;
            }
            finally
            {
                if (!isSilentMode)
                    Main.SetFinishedLoading();
            }
        }
    }
}