using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Web_153505_Shevtsova_D.Domain.Entities;

public class Tea
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    //public int CategoryId { get; set; }

    public TeaBasesCategory? Category { get; set; }

    public int Price { get; set; }

    public string? PhotoPath { get; set; }

    public string MIMEType { get; set; }
}
