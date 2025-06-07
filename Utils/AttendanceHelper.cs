using TTDAttendancePortal_backend.Data;
using TTDAttendancePortal_backend.Models.Entities;

namespace TTDAttendancePortal_backend.Utils
{
    public class AttendanceHelper
    {
        private readonly ApplicationDbContext _dbContext;

        public AttendanceHelper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void FillAbsentDays(int userId)
        {
            var currentDate = DateTime.Today;

            var lastCompletedAttendance = _dbContext.Attendance
                .Where(a => a.UserId == userId && a.CheckInDate.HasValue && a.CheckOutDate.HasValue)
                .OrderByDescending(a => a.CheckInDate)
                .FirstOrDefault();

            var referenceDate = lastCompletedAttendance?.CheckInDate?.Date ?? currentDate;
            var gapDays = (currentDate - referenceDate).Days;

            if (gapDays > 1)
            {
                for (int i = 1; i < gapDays; i++)
                {
                    var missedDate = referenceDate.AddDays(i);

                    bool alreadyExists = _dbContext.Attendance.Any(a =>
                        a.UserId == userId &&
                        a.CheckInDate.HasValue &&
                        a.CheckInDate.Value.Date == missedDate.Date);

                    if (!alreadyExists)
                    {
                        string status = missedDate.DayOfWeek == DayOfWeek.Sunday ? "Sunday" : "Absent";

                        _dbContext.Attendance.Add(new Attendance
                        {
                            UserId = userId,
                            CheckInDate = missedDate,
                            Status = status
                        });
                    }
                }

                _dbContext.SaveChanges();
            }
        }
    }
}
