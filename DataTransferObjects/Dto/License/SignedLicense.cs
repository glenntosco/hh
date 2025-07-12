namespace Pro4Soft.DataTransferObjects.Dto.License
{
    public class SignedLicense
    {
        public string Signature { get; set; }
        public string Payload { get; set; }
        public string Key { get; set; }
    }
}