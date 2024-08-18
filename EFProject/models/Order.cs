
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DXAppProject.models;

internal class Order
{
    [Key]
    public int ID { get; set; }
    public int CustomerID { get; set; }
    public DateTime? OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? RequiredDate { get; set; } = DateTime.UtcNow.AddDays(7);
    public DateTime? ShippedDate { get; set; } = DateTime.UtcNow.AddDays(1);
    public string Status { get; set; } = "Pending";
    [MaxLength(255)]
    public string? Comments { get; set; }
    [ForeignKey("CustomerID")]
    public Customer customer { get; set; }
}
