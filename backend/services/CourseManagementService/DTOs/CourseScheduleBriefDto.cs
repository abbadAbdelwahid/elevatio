namespace CourseManagementService.DTOs
{
    public class CourseScheduleBriefDto
    {
        public int CourseScheduleId { get; set; }
        public string Day { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int ModuleId { get; set; }
        public string Location { get; set; }
        public int ScheduleId { get; set; }
        public string TeacherFullName { get; set; }

    }
}