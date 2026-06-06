using System.ComponentModel.DataAnnotations;
namespace BlazorLesson1.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        [Required]
        public string StudentNumber { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [Range(1, 8)]
        public int Semester {  get; set; }
    }
}
