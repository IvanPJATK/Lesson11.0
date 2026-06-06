using BlazorLesson1.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlazorLesson1.Controllers
{
    [ApiController]
    [Route("api")]
    public class StudentController : ControllerBase
    {
        private static readonly List<StudentDto> Students = new()
        {
            new StudentDto { Id = 1, StudentNumber = "s32785", FirstName = "John", LastName = "Doe", Email = "john.doe@edu.pl", Semester = 4 },
            new StudentDto { Id = 2, StudentNumber = "s48390", FirstName = "Will", LastName = "Smith", Email = "will.smith@edu.pl", Semester = 2 }
        };

        private static readonly List<CourseDto> Courses = new()
        {
            new CourseDto { Id = 101, Name = "Application Programming (APBD)", Ects = 5 },
            new CourseDto { Id = 102, Name = "Database Systems", Ects = 4 },
            new CourseDto { Id = 103, Name = "Advanced Web Development", Ects = 6 }
        };

        private static readonly List<StudentCourseDto> StudentCourses = new()
        {
            new StudentCourseDto { StudentId = 1, CourseId = 101, AssignedAt = DateTime.Now.AddDays(-10) }
        };

        [HttpGet("students")]
        public IActionResult GetStudents() => Ok(Students);
        [HttpGet("students/{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = Students.FirstOrDefault(s => s.Id == id);
            if(student == null) return NotFound();
            var assignedCourseIds = StudentCourses.Where(sc => sc.StudentId == id).Select(sc => sc.CourseId).ToList();
            var mathchedCourses = Courses.Where(c => assignedCourseIds.Contains(c.Id)).ToList();
            return Ok(new { Student = student, Courses = mathchedCourses});
        }
        [HttpPost("students")]
        public IActionResult CreateStudent([FromBody] StudentDto dto)
        {
            dto.Id = Students.Any() ? Students.Max(s => s.Id) + 1 : 1;
            Students.Add(dto);
            return CreatedAtAction(nameof(GetStudentById), new {id = dto.Id});
        }
        [HttpGet("courses")]
        public IActionResult GetCourses() => Ok(Courses);
        [HttpPost("students/{id}/courses")]
        public IActionResult AssignCourse(int id, [FromBody]int courseId)
        {
            if (!Students.Any(s => s.Id == id)) return NotFound("Student not found");
            if (!Courses.Any(c => c.Id == courseId)) return BadRequest("No such course");
            if (StudentCourses.Any(sc => sc.CourseId == courseId && sc.StudentId == id)) return BadRequest("Course already assigned");
            StudentCourses.Add(new StudentCourseDto {CourseId = courseId, StudentId = id, AssignedAt = DateTime.Now});
            return NoContent();
        }
    }
}
