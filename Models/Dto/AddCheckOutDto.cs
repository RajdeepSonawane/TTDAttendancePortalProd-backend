namespace TTDAttendancePortal_backend.Models.Dto
{
    public class AddCheckOutDto
    {
        public int CheckOutLocation { get; set; }
        public string? CheckOutOtherLocation { get; set; }
        public string? Activity { get; set; }
    }
}
