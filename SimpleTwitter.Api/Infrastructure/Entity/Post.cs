using System.ComponentModel.DataAnnotations;

namespace SimpleTwitter.Api.Infrastructure.Entity;

public class Post
{
    [Key]
    public long Id { get; set; }
    
    [Required(ErrorMessage = "Content field is required")]
    [MaxLength(140, ErrorMessage = "Content field maximum length is 140")]
    [MinLength(1, ErrorMessage = "Content field minimum length is 1")]
    public string Content { get; set; }
    
    [Required(ErrorMessage = "User field is required")]
    [MinLength(3, ErrorMessage = "User field minimum length is 3")]
    public string Username { get; set; }
    
    public DateTime CreatedDate { get; set; }
}