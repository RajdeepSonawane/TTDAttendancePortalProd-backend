using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTDAttendancePortal_backend.Models.Entities
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int Id { get; set; }

        public  int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public DateTime? CheckInDate { get; set; }

        public  int? CheckInLocation { get; set; }
        [ForeignKey("CheckInLocation")]
        public Location? CheckInLocationNav { get; set; }

        public  string? CheckInOtherLocation { get; set; }

        public  DateTime? CheckOutDate { get; set; }

        public  int? CheckOutLocation { get; set; }
        [ForeignKey("CheckOutLocation")]
        public Location? CheckOutLocationNav { get; set; }

        public string? CheckOutOtherLocation { get; set; }


        public string? Activity { get; set; }

        public   string Status { get; set; }
        public TimeSpan? HoursWorked { get; set; }

        
    }
}
