using System;
using System.Collections.Generic;
using System.IO;

namespace Pro4Soft.DataTransferObjects.Dto.Generic
{
    public class LoginDetails
    {
        public Guid? TenantId { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? UserSessionId { get; set; }
        public Guid? DefaultWarehouseId { get; set; }
        public UserType UserType { get; set; }
        public bool AgreementSigned { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string LanguageId { get; set; }
        public int NewMessages { get; set; }
        public List<Menu> Menu { get; set; } = new List<Menu>();
        public string ExpiryDateFormat { get; set; }
        public ScanType TenantScanType { get; set; }
        public ScanType? UserScanType { get; set; }
        public bool? TrackGeoLocation { get; set; }

        public ScanType GetScanType()
        {
            if (TenantScanType != ScanType.UserSpecific)
                return TenantScanType;
            return UserScanType ?? ScanType.ZebraDataWedge;
        }
    }

    public class Menu
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string State { get; set; }
        public Dictionary<string, object> StateParams { get; set; }
        public string Icon { get; set; }
        public List<Menu> Children { get; set; } = new List<Menu>();

        public Menu Parent { get; set; }

        private readonly Lazy<Dictionary<string, string>> _icons = new Lazy<Dictionary<string, string>>(() =>
        {
            string json;
            using (var reader = new StreamReader(typeof(Menu).Assembly.GetManifestResourceStream("Pro4Soft.DataTransferObjects.Resources.FontMap.json")))
                json = reader.ReadToEnd();
            return Utils.DeserializeFromJson<Dictionary<string, string>>(json);
        });

        public string IconText => _icons.Value.TryGetValue(Icon, out var val) ? val : "\uf02a";

        public static Menu GetById(IEnumerable<Menu> children, string id)
        {
            foreach (var child in children)
            {
                if (child.Id == id)
                    return child;
                var rez = GetById(child.Children, id);
                if (rez != null)
                    return rez;
            }

            return null;
        }
    }
}