namespace TTDAttendancePortal_backend.Models.Dto
{
    public class MinimalAttendanceDto
    {
        public string Status { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
    }
}
