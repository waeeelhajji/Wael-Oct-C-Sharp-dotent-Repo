#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace LogAndRegLecture.Models;

public class LoginUser
{

    [EmailAddress]
    [Required]
    public string LogEmail { get; set; }
    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string LogPassword { get; set; }
}


