using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TTDAttendancePortal_backend.Data;
using TTDAttendancePortal_backend.Models.Dto;
using TTDAttendancePortal_backend.Models.Entities;
using TTDAttendancePortal_backend.Utils;

namespace TTDAttendancePortal_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AttendanceController : Controller
    {

        private readonly ApplicationDbContext dbContext;
        private readonly AttendanceHelper _attendanceHelper;

        public AttendanceController(ApplicationDbContext dbContext, AttendanceHelper attendanceHelper)
        {
            this.dbContext = dbContext;
            _attendanceHelper = attendanceHelper;
        }

        //[Authorize(Roles = "Super Admin")]
        [HttpGet("attendance-list/{id}")]
        public IActionResult GetAttendanceData(int id)
        {
            var attendance = dbContext.Attendance.FirstOrDefault(a => a.Id == id);
            if (attendance == null)
                return NotFound($"attendance with ID {id} not found");

            return Ok(attendance);
        }


        [Authorize]
        [HttpPost("check-in")]
        public IActionResult AddAttendance(AddCheckInDto checkInDto)
        {


            var userIdClaim = HttpContext.User.FindFirst("Id");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token");

            int UserId = int.Parse(userIdClaim.Value);


            var currentDate = DateTime.Today;

            var alreadyCheckedInToday = dbContext.Attendance
            .Any(a => a.UserId == UserId && a.CheckInDate.HasValue && a.CheckInDate.Value.Date == currentDate);

            if (alreadyCheckedInToday)
            {
                return Conflict("You have already checked in today.");
            }

            string status = "Partial";

            var attendanceEntity = new Attendance()
            {
                UserId = UserId,
                CheckInDate = DateTime.Now,
                CheckInLocation = checkInDto.CheckInLocation,
                CheckInOtherLocation = checkInDto.CheckInLocation == 1 ? checkInDto.CheckInOtherLocation : null,
                CheckOutDate = null,
                CheckOutLocation = null,
                CheckOutOtherLocation = null,
                Activity = null,
                Status = status,
                HoursWorked = null,
            };

            dbContext.Attendance.Add(attendanceEntity);
            dbContext.SaveChanges();

            return Ok(attendanceEntity);
        
        }

        [Authorize]
        [HttpPut("check-out")]
        public IActionResult UpdateAttendance(AddCheckOutDto checkOutDto)
        {


            var userIdClaim = HttpContext.User.FindFirst("Id");

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token");

            int UserId = int.Parse(userIdClaim.Value);

            DateTime checkOutDate = DateTime.Now;

            var attendance = dbContext.Attendance
                                .Where(a => a.UserId == UserId && a.CheckOutDate == null)
                                .OrderByDescending(a => a.CheckInDate)
                                .FirstOrDefault();

            if (attendance == null)
                return NotFound("No active attendance found to update");

            string stat = "Partial";
            if (attendance.CheckInDate.HasValue && attendance.CheckInDate.Value.Date == checkOutDate.Date)
            {
                stat = "Present";
            }

            if ( attendance.CheckInDate.HasValue)
            {
                attendance.HoursWorked = checkOutDate - attendance.CheckInDate.Value;
            }

            attendance.CheckOutDate = checkOutDate;
            attendance.CheckOutLocation = checkOutDto.CheckOutLocation;
            attendance.CheckOutOtherLocation = checkOutDto.CheckOutLocation == 1 ? checkOutDto.CheckOutOtherLocation : null;
            attendance.Activity = checkOutDto.Activity;
            attendance.Status = stat;
          
            dbContext.SaveChanges();

            return Ok(attendance);


        }


        //[Authorize]
        /*[HttpPost("fill-absent-gaps")]
        public IActionResult FillAbsentDays(int id)
        {
            //var userIdClaim = HttpContext.User.FindFirst("Id");

            //if (userIdClaim == null)
            //  return Unauthorized("User ID not found in token");

            // int UserId = int.Parse(userIdClaim.Value);
            int UserId = id;
            var currentDate = DateTime.Today;

            // Use the last attendance that has both check-in and check-out (i.e., completed day)
            var lastCompletedAttendance = dbContext.Attendance
                .Where(a => a.UserId == UserId && a.CheckInDate.HasValue && a.CheckOutDate.HasValue)
                .OrderByDescending(a => a.CheckInDate)
                .FirstOrDefault();

            var referenceDate = lastCompletedAttendance?.CheckInDate?.Date ?? currentDate;

            var gapDays = (currentDate - referenceDate).Days;

            if (gapDays > 1)
            {
                for (int i = 1; i < gapDays; i++)
                {
                    var missedDate = referenceDate.AddDays(i);

                    bool alreadyExists = dbContext.Attendance.Any(a =>
                        a.UserId == UserId &&
                        a.CheckInDate.HasValue &&
                        a.CheckInDate.Value.Date == missedDate.Date);

                    if (!alreadyExists)
                    {
                        string status = missedDate.DayOfWeek == DayOfWeek.Sunday ? "Sunday" : "Absent";

                        dbContext.Attendance.Add(new Attendance
                        {
                            UserId = UserId,
                            CheckInDate = missedDate,
                            Status = status
                        });
                    }
                }

                dbContext.SaveChanges();
            }

            return Ok("Absent days filled.");
        }*/



        [Authorize]
        [HttpPut("check-out-previous")]
        public IActionResult UpdatePreviousCheckOut(AddCheckOutPreviousDto checkOutPreviousDto)
        {
            var userIdClaim = HttpContext.User.FindFirst("Id");
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token");

            int userId = int.Parse(userIdClaim.Value);

            

            // Find attendance record by date sent from frontend
            var attendance = dbContext.Attendance
                .Where(a => a.UserId == userId && a.CheckInDate.HasValue)
                .OrderByDescending(a => a.CheckInDate)
                .FirstOrDefault(a => a.CheckInDate.Value.Date == checkOutPreviousDto.CheckOutDateTime.Date);

            if (attendance == null)
                return NotFound("No attendance found for the given date");

            var localTime = TimeZoneInfo.ConvertTimeFromUtc(checkOutPreviousDto.CheckOutDateTime, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            attendance.CheckOutDate = localTime;

            string stat = "Partial";
            if (attendance.CheckInDate.HasValue && attendance.CheckInDate.Value.Date == checkOutPreviousDto.CheckOutDateTime.Date)
            {
                stat = "Present";
            }

            if (attendance.CheckInDate.HasValue)
            {
                attendance.HoursWorked = checkOutPreviousDto.CheckOutDateTime - attendance.CheckInDate.Value;
                

            }

            attendance.CheckOutDate = checkOutPreviousDto.CheckOutDateTime;
            attendance.CheckOutLocation = checkOutPreviousDto.CheckOutLocation;
            attendance.CheckOutOtherLocation = checkOutPreviousDto.CheckOutLocation == 1 ? checkOutPreviousDto.CheckOutOtherLocation : null;
            attendance.Activity = checkOutPreviousDto.Activity;
            attendance.Status = stat;

            dbContext.SaveChanges();

            return Ok(attendance);
        }



        //[Authorize(Roles = "Super Admin")]
        [HttpGet("user-attendence/{id}")]
        public IActionResult GetUserAttendanceData(int id)
        {
            _attendanceHelper.FillAbsentDays(id);

            var attendance = dbContext.Attendance
                                .Where(a => a.UserId == id && a.CheckOutDate == null && a.Status=="Partial")
                                .OrderBy(a => a.CheckInDate)
                                   .Select(a => new MinimalAttendanceDto
                                   {
                                       CheckInDate = a.CheckInDate.Value,
                                       CheckOutDate = a.CheckOutDate,
                                       Status=a.Status
                                   })
                                    .FirstOrDefault();
            if (attendance == null)
                return NotFound($"attendance with ID {id} not found");

            return Ok(attendance);
        }
    }
}
