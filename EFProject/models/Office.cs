using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DXAppProject.models;

internal class Office
{
    [Key]
    public int Code { get; set; }
    [MaxLength(255)]
    public string? City { get; set; }
    [MaxLength(255)]
    public string? Phone { get; set; }
    [MaxLength(255)]
    public string? Address { get; set; }
    [MaxLength(255)]
    public int? Postalcode { get; set; }
    public List<Employee> employees { get; set; }

}
