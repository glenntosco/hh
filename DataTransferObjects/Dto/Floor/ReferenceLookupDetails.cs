using System;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class ReferenceLookupDetails:GenericMessage
    {
        public Guid? ClientId { get; set; }
        public string Client { get; set; }
        public string ReferenceCode { get; set; }
    }
}