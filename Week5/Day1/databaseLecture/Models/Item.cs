#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace databaseLecture.Models;

public class Item
{
    // We need an ID
    [Key]
    [Required]
    public int ItemId { get; set; }
    [Required]
    [MinLength(10)]
    public string Name { get; set; }
    public string Description { get; set; }

    // We always include a createdAt and updatedAt because i't is good practice
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}