using System.ComponentModel.DataAnnotations;

namespace OrderReportFunction.Models;

public class Order
{
    [Required]
    public string OrderId { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal VAT { get; set; }
    public decimal TotalWithVAT => TotalPrice + VAT;
}
