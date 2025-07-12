using System;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class CustomAction
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ReferenceType { get; set; }
        public bool QtyRequired { get; set; }
    }
}