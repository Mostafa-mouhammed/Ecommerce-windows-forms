
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DXAppProject.models;

internal class Payment
{
    [Key]
    public int CheckNum { get; set; }
    public int CustomerID { get; set; }
    public DateTime? PaymentDate { get; set; } = DateTime.UtcNow;
    public decimal? Amount { get; set; }
    [ForeignKey("CustomerID")]
    public Customer customer { get; set; }
}
