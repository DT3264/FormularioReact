using System.ComponentModel.DataAnnotations;

namespace aspnetcore_with_reactspa.Models;

public class Pizza
{
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string? Name { get; set; }

    public Sauce? Sauce { get; set; }
    
    public ICollection<Topping>? Toppings { get; set; }
}