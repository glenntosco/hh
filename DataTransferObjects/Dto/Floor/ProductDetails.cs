using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming

namespace Pro4Soft.DataTransferObjects.Dto.Floor
{
    public class ProductDetails
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Category { get; set; }
        public string Barcode { get; set; }
        public string ClientName { get; set; }
        public Guid? ClientId { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public Guid? PacksizeId { get; set; }
        public string PacksizeName { get; set; }
        public int? EachCount { get; set; }
        public List<Packsize> Packsizes { get; set; } = new List<Packsize>();

        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }

        public bool IsSerialControlled { get; set; }
        public bool IsLotControlled { get; set; }
        public bool IsExpiryControlled { get; set; }
        public bool IsPacksizeControlled { get; set; }
        public bool IsDecimalControlled { get; set; }

        public bool IsDetailControlled => IsSerialControlled || IsLotControlled || IsExpiryControlled || IsPacksizeControlled;

        public UnitOfMeasure? UnitOfMeasure { get; set; }
        public UnitOfMeasure? WeightUnitOfMeasure { get; set; }
        public UnitOfMeasure? LengthUnitOfMeasure { get; set; }
    }

    public class Packsize
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int EachCount { get; set; }

        public string Barcode { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }
    }

    public class ProductAvailability
    {
        public int TotalCount { get; set; }
        public List<ProductAvailabilityRecord> Records { get; set; } = new List<ProductAvailabilityRecord>();

        public string GetOnHandQtyText(Func<string, string> translate)
        {
            return translate($"Showing [{Records.Count}/{TotalCount}] records\n") +
                   string.Join("\n", Records
                       .OrderBy(c => c.WarehouseCode)
                       .ThenBy(c => c.TotalQuantity)
                       .ThenBy(c => c.BinCode)
                       .ThenBy(c => c.LicensePlateCode)
                       .Select(c => c.GetOnHandQtyText(translate)));
        }

        public string GetCycleCountText(Func<string, string> translate)
        {
            var result = string.Join("\n", Records
                             .OrderBy(c => c.WarehouseCode)
                             .ThenByDescending(c => string.IsNullOrWhiteSpace(c.BinCode))
                             .ThenBy(c => c.LicensePlateCode)
                             .Select(c => c.GetCycleCountText(translate)));
            return result;
        }
    }

    public class ProductAvailabilityRecord
    {
        public Guid ProductId { get; set; }
        public string Sku { get; set; }
        public string Upc { get; set; }
        public string Category { get; set; }

        public string ReferenceNumber { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }

        public string SubstituteGroup { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseCode { get; set; }
        public decimal? OpenQuantity { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? AllocatedQuantity { get; set; }
        public string UnitOfMeasure { get; set; }

        public string BinCode { get; set; }
        public Guid? BinId { get; set; }

        public string LicensePlateCode { get; set; }
        public Guid? LicensePlateId { get; set; }

        public string GetOnHandQtyText(Func<string, string> translate)
        {
            var result = $"[{BinCode}]";
            if (!string.IsNullOrEmpty(LicensePlateCode))
            {
                result = LicensePlateCode;
                if (!string.IsNullOrEmpty(BinCode))
                    result = $"[{LicensePlateCode}@{BinCode}]";
            }

            return $"{result} - {translate($"Total [{TotalQuantity}], OnHand [{TotalQuantity - AllocatedQuantity}]")}";
        }

        public string GetCycleCountText(Func<string, string> translate)
        {
            var result = BinCode;
            if (!string.IsNullOrEmpty(LicensePlateCode))
                result = !string.IsNullOrEmpty(BinCode) ? $"{LicensePlateCode}@{BinCode}" : $"{LicensePlateCode}@{translate("Floor")}";

            return $"{result} - {translate($"Qty [{TotalQuantity}]")}";
        }
    }

    public enum UnitOfMeasure
    {
        Gr,
        Kg,

        Oz,
        Lb,

        Ml,
        L,

        Pt,
        Gal,

        Mm,
        Cm,
        M,

        In,
        Ft
    }

    public enum FreightClass
    {
        Class50,
        Class55,
        Class60,
        Class65,
        Class70,
        Class77p5,
        Class85,
        Class92p5,
        Class100,
        Class110,
        Class125,
        Class150,
        Class175,
        Class200,
        Class250,
        Class300,
        Class400,
        Class500,
    }

    public enum CycleCountStyle
    {
        ByBin,
        ByProduct
    }

    public enum CartonizationBehaviour
    {
        Inherit,
        PreBoxed,
        SingleSku,
        MultiSku
    }

    public enum BarcodeType
    {
        Code128,
        ITF14,
        EAN13,
        UPCA,
    }
}