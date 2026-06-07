using BlazorLesson1.DTOs;

namespace BlazorLesson1.Services
{
    public class StateContainer
    {
        private readonly List<StudentDto> _observedStudents = new();
        public IReadOnlyList<StudentDto> ObservedStudents => _observedStudents.AsReadOnly();
        public event Action? OnChange;
        public void ToggleObservation(StudentDto student)
        {
            var existing = _observedStudents.FirstOrDefault(s => s.Id == student.Id);
            if (existing != null)
            {
                _observedStudents.Remove(existing);
            }
            else
            {
                _observedStudents.Add(student);
            }
            NotifyStateChanged();
        }
        public bool IsObserved(int studentId) => _observedStudents.Any(s => s.Id == studentId);
        private void NotifyStateChanged() => OnChange?.Invoke();

    }
}
