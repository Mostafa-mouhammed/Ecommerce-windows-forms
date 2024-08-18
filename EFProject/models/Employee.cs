
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Documents;

namespace DXAppProject.models;

internal class Employee
{
    [Key]
    public int ID { get; set; }
    public int OfficeCode { get; set; }
    public int? reportsto { get; set; }
    [MaxLength(255)]
    public string? FirstName { get; set; }
    [MaxLength(255)]
    public string? LastName { get; set; }
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
    [MaxLength(255)]
    public string? Email { get; set; }
    [ForeignKey("OfficeCode")]
    public Office office { get; set; }
    [ForeignKey("reportsto")]
    public Employee? employee { get; set; }
    public List<Employee>? employees { get; set; }
    public List<Customer> customers { get; set; }

}
