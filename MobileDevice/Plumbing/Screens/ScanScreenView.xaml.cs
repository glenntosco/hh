using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Dto;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.DataTransferObjects.Dto.Floor;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.ScanMessages;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Pro4Soft.MobileDevice.Plumbing.Screens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanScreenView : BaseContentView
    {
        #region Setup and Header/Toolbar
        public ScanScreenView()
        {
            InitializeComponent();
        }

        public override async Task OnApearing()
        {
            if (Controller != null)     
                await Controller?.InitBase();
        }

        public void SetTitle(string title)
        {
            Title.Text = Lang.Translate(title);
        }

        public override async void OnBackButtonPressed()
        {
            await Main.NavigateToView<MenuView>();
        }

        public void ClearToolbar()
        {
            ToolbarContainer.Children.Clear();
        }

        public Button AddToolbar(string label, Func<Task> action)
        {
            var newBtn = new Button
            {
                Text = Lang.Translate(label)
            };
            newBtn.Clicked += async (s, e) =>
            {
                await action.Invoke();
            };
            ToolbarContainer.Children.Add(newBtn);
            return newBtn;
        }

        public Button RemoveToolbar(Button toolBarItem)
        {
            if(toolBarItem != null)
                ToolbarContainer.Children.Remove(toolBarItem);
            return null;
        }
        #endregion

        #region MessageBox
        public void ClearMessages()
        {
            Container.Children.Clear();
        }

        public void InactivateMessages()
        {
            Container.Children.Cast<BaseScanMessage>().ForEach(c =>
            {
                c.Replay = null;
            });
        }

        public async Task PushThumbnailMessage(string message, string url, Func<Task> replay = null, bool translate = true)
        {
            if (translate)
                message = Lang.Translate(message);

            var resp = await Singleton<Web>.Instance.WebInvokeAsync($"{url}/medium?includeDefault=false");
            View newElement;

            if (resp.StatusCode == HttpStatusCode.OK && resp.RawBytes.Length > 0)
                newElement = new ThumbnailScanMessage(message, resp.RawBytes, replay, Container);
            else
                newElement = new TextScanMessage(message, replay, Container);

            Container.Children.Add(newElement);
            await Scroller.ScrollToAsync(newElement, ScrollToPosition.MakeVisible, false);
        }

        public void PushMessageWithSubtitle(string header, string body, string subtitle, Func<Task> action = null, bool translate = true)
        {
            if (translate)
                header = Lang.Translate(header);
            var newElement = new TextWithSubtitleMessage(header, body, subtitle, null, action);
            Container.Children.Add(newElement);
        }

        public async Task PushMessage(string message, Func<Task> replay = null, bool translate = true)
        {
            if(translate)
                message = Lang.Translate(message);
            var newElement = new TextScanMessage(message, replay, Container);
            Container.Children.Add(newElement);
            await Scroller.ScrollToAsync(newElement, ScrollToPosition.MakeVisible, false);
        }

        public async Task ScrollToBottom()
        {
            var last = Container.Children.LastOrDefault();
            if(last != null)
                await Scroller.ScrollToAsync(last, ScrollToPosition.MakeVisible, false);
        }

        public async Task PushChatMessage(UserMessage message)
        {
            var newElement = new TextWithSubtitleMessage(message.Message, null, message.FromUsername, message.Timestamp.ToString("hh:mm:ss t"));
            Container.Children.Add(newElement);
            await Scroller.ScrollToAsync(newElement, ScrollToPosition.MakeVisible, false);
        }

        public async Task PushImageMessage(byte[] bytes, Func<Task> replay = null)
        {
            var newElement = new ImageScanMessage(bytes, replay, Container);
            Container.Children.Add(newElement);
            await Scroller.ScrollToAsync(newElement, ScrollToPosition.MakeVisible, false);
        }

        public Task PopLastMessage()
        {
            if (Container.Children.Any())
                Container.Children.Remove(Container.Children.LastOrDefault());
            return Task.CompletedTask;
        }

        public async Task PushError(string message, Func<Task> replay = null)
        {
            var newElement = new ErrorScanMessage(message, replay, Container);
            Container.Children.Add(newElement);
            await Scroller.ScrollToAsync(newElement, ScrollToPosition.MakeVisible, false);
        }
        #endregion

        #region Prompts
        private void ClearPrompts()
        {
            ControlsContainer.HeightRequest = 50;
            ControlsContainer.Children.Clear();
        }

        public void PromptError(Exception ex)
        {
            ClearPrompts();
            ControlsContainer.Children.Add(new Label
            {
                Text = ex.Message,
                FontSize = 14,
                TextColor = Color.DarkRed,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            });
        }

        public void PromptInfo(string label, bool translate = true)
        {
            ClearPrompts();
            ControlsContainer.Children.Add(new Label
            {
                Text = translate?Lang.Translate(label):label,
                FontSize = 18,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            });
        }

        public Task<string> PromptScan(string label)
        {
            ClearPrompts();

            var scanType = Singleton<Context>.Instance.GetScanType();
            if(scanType == ScanType.LineFeed)
                return PromptString(label);

            var promise = new TaskCompletionSource<string>();
            MessagingCenter.Unsubscribe<App, string>(Application.Current, "ScanBarcode");
            MessagingCenter.Subscribe<App, string>(Application.Current, "ScanBarcode", (sender, arg) => Dispatcher.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Unsubscribe<App, string>(Application.Current, "ScanBarcode");
                ClearPrompts();
                promise.SetResult(arg);
            }));

            switch (scanType)
            {
                case ScanType.Camera:
                {
                    var scanView = new ZXingScannerView
                    {
                        IsScanning = true,
                        //WidthRequest = 150,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                    };
                    scanView.OnScanResult += result =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ClearPrompts();
                            promise.SetResult(result.Text);
                        });
                    };
                    ControlsContainer.HeightRequest = 160;
                    ControlsContainer.Children.Add(new Grid
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Children = { scanView, new ZXingDefaultOverlay { TopText = Lang.Translate(label) } }
                    });
                    break;
                }
                default:
                {
                    ControlsContainer.Children.Add(new Label
                    {
                        Text = Lang.Translate(label),
                        FontSize = 18,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    });
                    break;
                }
            }
            return promise.Task;
        }
        
        public Task<decimal?> PromptNumeric(string label, decimal? defaultVal = null)
        {
            ClearPrompts();

            var numericPromise = new TaskCompletionSource<decimal?>();
            ControlsContainer.Children.Add(new Label
            {
                MinimumWidthRequest = 100,
                FontSize = 18,
                Text = Lang.Translate(label),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            });

            var qtyEntry = new Entry
            {
                FontSize = 18,
                Keyboard = Keyboard.Numeric,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = defaultVal?.ToString(),
                
            };
            ControlsContainer.Children.Add(qtyEntry);
            qtyEntry.Completed += (s, e) =>
            {
                var control = (Entry)s;
                var qty = decimal.TryParse(control.Text, out var res) ? res : (decimal?)null;
                ClearPrompts();
                numericPromise.SetResult(qty);
            };
            qtyEntry.Focus();

            qtyEntry.CursorPosition = 0;
            qtyEntry.SelectionLength = defaultVal?.ToString().Length ?? 0;

            if (Singleton<Context>.Instance.AllowQtyScan)
            {
                MessagingCenter.Unsubscribe<App, string>(Application.Current, "ScanBarcode");
                MessagingCenter.Subscribe<App, string>(Application.Current, "ScanBarcode", (sender, scannedValue) => Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    if (!decimal.TryParse(scannedValue, out var result))
                        return;
                    MessagingCenter.Unsubscribe<App, string>(Application.Current, "ScanBarcode");
                    ClearPrompts();
                    numericPromise.SetResult(result);
                }));
            }

            return numericPromise.Task;
        }

        public Task<string> PromptString(string label)
        {
            ClearPrompts();

            var stringPromise = new TaskCompletionSource<string>();
            ControlsContainer.Children.Add(new Label
            {
                MinimumWidthRequest = 100,
                FontSize = 18,
                Text = Lang.Translate(label),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            });

            var strEntry = new Entry
            {
                FontSize = 18,
                Keyboard = Keyboard.Default,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            ControlsContainer.Children.Add(strEntry);
            strEntry.Completed += (s, e) =>
            {
                var control = (Entry)s;
                ClearPrompts();
                stringPromise.SetResult(control.Text);
            };
            strEntry.Focus();

            MessagingCenter.Unsubscribe<App, string>(Application.Current, "ScanBarcode");
            MessagingCenter.Subscribe<App, string>(Application.Current, "ScanBarcode", (sender, scannedValue) => Dispatcher.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Unsubscribe<App, string>(Application.Current, "ScanBarcode");
                ClearPrompts();
                stringPromise.SetResult(scannedValue);
            }));

            return stringPromise.Task;
        }

        public Task<bool> PromptBool(string label, string yes, string no = null)
        {
            ClearPrompts();

            var boolPromise = new TaskCompletionSource<bool>();
            ControlsContainer.Children.Add(new Label
            {
                MinimumWidthRequest = 100,
                FontSize = 18,
                Text = Lang.Translate(label),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            });

            var yesBtn = new Button
            {
                Text = Lang.Translate(yes),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            ControlsContainer.Children.Add(yesBtn);
            yesBtn.Clicked += (s, e) =>
            {
                ClearPrompts();
                boolPromise.SetResult(true);
            };

            if(no == null)
                return boolPromise.Task;

            var noBtn = new Button
            {
                Text = Lang.Translate(no),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            ControlsContainer.Children.Add(noBtn);
            noBtn.Clicked += (s, e) =>
            {
                ClearPrompts();
                boolPromise.SetResult(false);
            };

            return boolPromise.Task;
        }

        public async Task<Packsize> PromptPacksize(string label, List<Packsize> dataSource)
        {
            if(!dataSource.Any())
                throw new ExceptionLocalized($"No packsizes are setup");
            if (dataSource.Count == 1)
                return dataSource.First();
            var name = await PromptPicker(label, dataSource.OrderBy(c => c.EachCount).Select(c => $"x{c.EachCount}").ToList());
            return dataSource.First(c => name == $"x{c.EachCount}");
        }

        public Task<string> PromptPicker(string label, List<string> dataSource)
        {
            ClearPrompts();
            var pickerPromise = new TaskCompletionSource<string>();
            
            ControlsContainer.Children.Add(new Label
            {
                MinimumWidthRequest = 100,
                FontSize = 18,
                Text = Lang.Translate(label),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            });

            var qtyEntry = new Picker
            {
                FontSize = 18,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ItemsSource = dataSource
            };
            ControlsContainer.Children.Add(qtyEntry);
            qtyEntry.SelectedIndexChanged += (s, e) =>
            {
                var control = (Picker)s;
                if (control.SelectedItem == null)
                    return;
                ClearPrompts();
                pickerPromise.SetResult(control.SelectedItem as string);
            };
            qtyEntry.Focus();
            
            return pickerPromise.Task;
        }

        public Task<DateTime> PromptDate(string label)
        {
            ClearPrompts();

            var datePromise = new TaskCompletionSource<DateTime>();
            ControlsContainer.Children.Add(new Label
            {
                MinimumWidthRequest = 100,
                FontSize = 18,
                Text = Lang.Translate(label),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            });

            var datePicker = new DatePicker
            {
                FontSize = 18,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            ControlsContainer.Children.Add(datePicker);
            datePicker.DateSelected += (s, e) =>
            {
                var control = (DatePicker) s;
                ClearPrompts();
                datePromise.SetResult(control.Date);
            };
            datePicker.Focus();

            if (Singleton<Context>.Instance.AllowDateScan)
            {
                MessagingCenter.Unsubscribe<App, string>(Application.Current, "ScanBarcode");
                MessagingCenter.Subscribe<App, string>(Application.Current, "ScanBarcode", (sender, scannedValue) => Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    if (!DateTime.TryParseExact(scannedValue, Singleton<Context>.Instance.ExpiryDateFormat, null, DateTimeStyles.None, out var result))
                        return;
                    ClearPrompts();
                    MessagingCenter.Unsubscribe<App, string>(Application.Current, "ScanBarcode");
                    datePromise.SetResult(result);
                }));
            }

            return datePromise.Task;

        }
        #endregion

        public override void OnClosing()
        {
            base.OnClosing();
            Controller?.OnClosing();
        }
    }
}