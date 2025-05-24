using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rent.Dtos
{
    public class PaymentRequestDto
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public List<PackageDto> Packages { get; set; }
        public RedirectUrlsDto RedirectUrls { get; set; }
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public RequestOptionDto? Options { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
    }
    public class PackageDto
    {
        public string Id { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
        public List<LinePayProductDto> Products { get; set; }
        public int? UserFee { get; set; }

    }
    public class LinePayProductDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? Id { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? ImageUrl { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public int? OriginalPrice { get; set; }
    }

    public class RedirectUrlsDto
    {
        public string ConfirmUrl { get; set; }
        public string CancelUrl { get; set; }
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? AppPackageName { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? ConfirmUrlType { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
    }

    public class RequestOptionDto
    {
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public PaymentOptionDto? Payment { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public DisplpyOptionDto? Displpy { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public ShippingOptionDto? Shipping { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public ExtraOptionsDto? Extra { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
    }
    public class PaymentOptionDto
    {
        public bool? Capture { get; set; }
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? PayType { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
    }
    public class DisplpyOptionDto
    {
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? Local { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public bool? CheckConfirmUrlBrowser { get; set; }
    }
    public class ShippingOptionDto
    {
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? Type { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public int FeeAmount { get; set; }
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? FeeInquiryUrl { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? FeeInquiryType { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public ShippingAddressDto? Address { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
    }

    public class ShippingAddressDto
    {
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? Country { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? PostalCode { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? State { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? City { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? Detail { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? Optional { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public ShippingAddressRecipientDto Recipient { get; set; }
    }

    public class ShippingAddressRecipientDto
    {
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? FirstName { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? LastName { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? FirstNameOptional { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? LastNameOptional { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? Email { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? PhoneNo { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? Type { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
    }

    public class ExtraOptionsDto
    {
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? BranchName { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
#pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        public string? BranchId { get; set; }
#pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
    }
}