
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DXAppProject.models;

[PrimaryKey(nameof(OrderID), nameof(ProductCode))]

internal class Order_Product
{
    public int OrderID { get; set; }

    public int ProductCode { get; set; }
    public int? Qty { get; set; }
    public decimal? PriceEach { get; set; }
    [ForeignKey("OrderID")]
    public Order order { get; set; }
    [ForeignKey("ProductCode")]
    public Product product { get; set; }
}
