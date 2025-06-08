namespace CourseManagementService.DTOs;

public class ScheduleDto
{
    public string Group { get; set; }
    public string Year { get; set; }
    public string Week { get; set; }
    public List<CourseScheduleDto> Courses { get; set; }
}