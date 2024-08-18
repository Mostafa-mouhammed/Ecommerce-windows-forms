using DXAppProject.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFProject.models;

internal class Cart
{
    public Product product { get; set; }
    public int productQunt { get; set; }
}
