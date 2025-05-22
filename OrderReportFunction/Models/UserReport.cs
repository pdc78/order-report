using System.ComponentModel.DataAnnotations;

namespace OrderReportFunction.Models;

public class UserReport
{
    [Required]
    public string UserEmail { get; set; }
    public List<Order> Orders { get; set; } = new();
}
