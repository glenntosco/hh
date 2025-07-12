using System;
using System.Collections.Generic;

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class ProductLetdownRequest
    {
        public string BinCode { get; set; }
        public string Sku { get; set; }
        public Guid ProductId { get; set; }
        public Guid? BinId { get; set; }
        public string LicensePlate { get; set; }
        public decimal Quantity { get; set; }
        public string ImageUrl { get; set; }
        public int TotalMoves { get; set; }

        public List<string> SuggestedBins { get; set; } = new List<string>();
        public List<ProductOperation> Details { get; set; } = new List<ProductOperation>();
    }
}