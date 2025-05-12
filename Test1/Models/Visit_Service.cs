using System.ComponentModel.DataAnnotations;

namespace Test1.Models;

public class Visit_Service
{
    [MaxLength(100)]
    public string name { get; set; }
    [MaxLength(10)]
    public decimal service_fee { get; set; }
}