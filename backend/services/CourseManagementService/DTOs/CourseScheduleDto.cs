namespace CourseManagementService.DTOs;

public class CourseScheduleDto
{
    public string Day { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public int ModuleId { get; set; }
    public string Location { get; set; }
}