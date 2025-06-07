namespace TTDAttendancePortal_backend.Models.Dto
{
    public class AddCheckOutPreviousDto
    {
        public int CheckOutLocation { get; set; }
        public string? CheckOutOtherLocation { get; set; }
        public string? Activity { get; set; }
       
        public DateTime CheckOutDateTime { get; set; }

     
}
}
