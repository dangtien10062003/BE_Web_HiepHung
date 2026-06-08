using System.ComponentModel.DataAnnotations;

namespace MyHiep.Api.Models;

public class StoreSettings
{
    public int Id { get; set; }
    [MaxLength(160)] public string BrandName { get; set; } = "Giặt Sấy Hiệp Hưng";
    [MaxLength(300)] public string Address { get; set; } = "Địa chỉ cửa hàng đang cập nhật";
    [MaxLength(30)] public string Hotline { get; set; } = "0900 000 000";
    [MaxLength(200)] public string ZaloUrl { get; set; } = "https://zalo.me/0900000000";
    [MaxLength(200)] public string FacebookUrl { get; set; } = "https://facebook.com/";
    [MaxLength(120)] public string OpeningHours { get; set; } = "7:00 - 21:00 hằng ngày";
    [MaxLength(300)] public string DeliveryPolicy { get; set; } = "Hỗ trợ giao nhận trong bán kính 3km";
}
