namespace TTDAttendancePortal_backend.Models.Dto
{
    public class AddAttendanceDto

    {
        public  DateTime? CheckInDate { get; set; }
        public int CheckInLocation { get; set; }

        public string? CheckInOtherLocation { get; set; }
        public  DateTime? CheckOutDate { get; set; }

        public int CheckOutLocation { get; set; }
        public string? CheckOutOtherLocation { get; set; }
        public string? Activity { get; set; }
    }
}
