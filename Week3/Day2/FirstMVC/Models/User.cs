using System.ComponentModel.DataAnnotations;
namespace FirstMVC.Models;
#pragma warning disable CS8618


public class User
{
    [Required()]
    [MinLength(2)]
    public string Name { get; set; }
    [Required(ErrorMessage = "the favorite color is required")]
    public string FavColor { get; set; }
    [Required()]
    [Range(-1000, 1000)]
    public int FavNumber { get; set; }
}