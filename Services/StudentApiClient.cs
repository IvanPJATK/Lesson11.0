using BlazorLesson1.DTOs;

namespace BlazorLesson1.Services
{
    public class StudentApiClient
    {
        private readonly HttpClient _http;
        public StudentApiClient(HttpClient http) { _http = http; }
        public async Task<List<StudentDto>> GetStudentsAsync() =>
            await _http.GetFromJsonAsync<List<StudentDto>>("api/students") ?? new();
        public async Task<StudentDetailsPayload?> GetStudentDetailsAsync(int id)
        {
            var response = await _http.GetAsync($"api/students/{id}");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<StudentDetailsPayload>();
        }

        public class StudentDetailsPayload
        {
            public StudentDto Student { get; set; } = null!;
            public List<CourseDto> Courses { get; set; } = new();
        }
        public async Task<StudentDto?> CreateStudentAsync(StudentDto student)
        {
            var response = await _http.PostAsJsonAsync("api/students", student);
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<StudentDto>();
            return null;
        }

        public async Task<List<CourseDto>> GetCoursesAsync() =>
            await _http.GetFromJsonAsync<List<CourseDto>>("api/courses") ?? new();

        public async Task<bool> AssignCourseAsync(int studentId, int courseId)
        {
            var response = await _http.PostAsJsonAsync($"api/students/{studentId}/courses", courseId);
            return response.IsSuccessStatusCode;
        }
    }
}
