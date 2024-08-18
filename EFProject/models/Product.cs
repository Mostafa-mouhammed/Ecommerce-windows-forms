
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DXAppProject.models;

internal class Product
{
    [Key]
    public int Code { get; set; }
    public int Categoryid { get; set; }
    [MaxLength(255)]
    public string? Name { get; set; }
    [MaxLength(255)]
    public string? PdtDescription { get; set; }
    public int? QtyInStock { get; set; }
    public decimal? BuyPrice { get; set; }
    [ForeignKey("Categoryid")]
    public Category Category { get; set; }

    public static implicit operator int(Product v)
    {
        throw new NotImplementedException();
    }
}
