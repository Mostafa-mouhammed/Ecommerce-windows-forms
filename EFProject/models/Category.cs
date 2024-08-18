
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Documents;

namespace DXAppProject.models;
[Table("Product_line")]
internal class Category
{
    [Key]
    public int ID { get; set; }
    [MaxLength(255)]
    public string? Name { get; set; }
    [MaxLength(255)]
    public string? image { get; set; }
    public List<Product> products { get; set; }

}
