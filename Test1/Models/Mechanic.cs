using System.ComponentModel.DataAnnotations;

namespace Test1.Models;

public class Mechanic
{
    public int mechanic_id { get; set; }
    [MaxLength(14)]
    public string licence_number { get; set; }
}