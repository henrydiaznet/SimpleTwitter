using System.ComponentModel.DataAnnotations;

namespace SimpleTwitter.Api.Contracts;

public class AddPostRequest
{
    [Required(ErrorMessage = "Content is required")]
    [MaxLength(140, ErrorMessage = "Content cannot be longer than 140 characters")]
    [MinLength(1, ErrorMessage = "Content cannot be shorter than 1 character")]
    public string Content { get; set; }
    
    [Required(ErrorMessage = "Username is required")]
    [MinLength(3, ErrorMessage = "Username cannot be shorter than 3 characters")]
    public string Username { get; set; }
}