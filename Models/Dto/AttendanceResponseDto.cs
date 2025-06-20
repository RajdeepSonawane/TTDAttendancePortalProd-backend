namespace TTDAttendancePortal_backend.Models.Dto
{
    public class AttendanceResponseDto
    {
    public required string EmployeeCode { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime InDate { get; set; }
    public required string InTime { get; set; }
    public required string InLocation { get; set; }
    public required string OutTime { get; set; }
    public required string OutLocation { get; set; }

   
    public required string Status { get; set; }
    public required string Activity { get; set; }
    public required TimeSpan HoursWorked { get; set; }
    }
}
