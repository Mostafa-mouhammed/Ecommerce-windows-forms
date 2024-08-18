
using Microsoft.EntityFrameworkCore;
using DXAppProject.models;

namespace DXAppProject.DBContext;

internal class MyContext:DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=./;database=EfProject;trusted_connection=true;Encrypt=false;");
    }

   public DbSet<Office> offices { get; set; }
   public DbSet<Customer> customers { get; set; }
   public DbSet<Employee> employees { get; set; }
   public DbSet<Order> orders { get; set; }
   public DbSet<Payment> payment { get; set; }
   public DbSet<Product> products { get; set; }
   public DbSet<Category> Category { get; set; }
   public DbSet<Order_Product> Order_Product { get; set; }

}
