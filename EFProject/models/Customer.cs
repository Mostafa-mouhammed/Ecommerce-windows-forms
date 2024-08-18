
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DXAppProject.models;

internal class Customer
{
    [Key]
    public int ID { get; set; }
    public int SalesRepEmployeeNum { get; set; }
    [MaxLength(255)]
    public string? FirstName { get; set; }
    [MaxLength(255)]
    public string? LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    [MaxLength(255)]
    public string? Phone { get; set; }
    [MaxLength(255)]
    public string? City { get; set; }
    public List<Payment> Payment { get; set; }
    public List<Order> orders { get; set; }
    [ForeignKey("SalesRepEmployeeNum")]
    public Employee employee { get; set; }
}
