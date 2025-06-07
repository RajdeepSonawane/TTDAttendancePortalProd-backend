using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTDAttendancePortal_backend.Data;
using TTDAttendancePortal_backend.Models.Dto;
using TTDAttendancePortal_backend.Models.Entities;

namespace TTDAttendancePortal_backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //[Authorize(Roles = "Super Admin")]
        [HttpGet("user-list/{id}")]
        public IActionResult GetUserData(int id)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"User with ID {id} not found");

            return Ok(user);
        }

        //[Authorize(Roles = "Super Admin")]
        [HttpGet("user-list")]
        public IActionResult GetUsers()
        {
            var users = dbContext.Users.ToList();


            return Ok(users);
        }



        [Authorize]
        [Authorize(Roles = "Super Admin")]
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUsers([FromForm] AddUsersDto usersDto)
        {
            var identity = HttpContext.User.Identity;

            if (identity != null && identity.IsAuthenticated)
            {

                 var createdBy = HttpContext.User.FindFirst("UserId")?.Value;

                // Read file into byte array
               //byte[] imageData = null!;
               // using (var ms = new MemoryStream())
              //  {
              //      await usersDto.EmployeeImage.CopyToAsync(ms);
               //     imageData = ms.ToArray();
               // }

                // Hash password before storing
                var saltRounds = 10;
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(usersDto.Password, saltRounds);

                var userEntity = new User()
                {
                    //EmployeeImage= imageData,
                    EmployeeCode = usersDto.EmployeeCode,
                    FirstName = usersDto.FirstName,
                    MiddleName = usersDto.MiddleName,
                    LastName = usersDto.LastName,
                    ContactNumber = usersDto.ContactNumber,
                    CurrentAddress = usersDto.CurrentAddress,
                    Email = usersDto.Email,
                    Password = hashedPassword,
                    Designation = usersDto.Designation,
                    Role = usersDto.Role,
                    BloodGroup = usersDto.BloodGroup,
                    YearOfExp = usersDto.YearOfExp,
                    DOB = usersDto.DOB,
                    DOJ = usersDto.DOJ,
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.Now,
                  

                };

                dbContext.Users.Add(userEntity);
                dbContext.SaveChanges();

                return Ok(userEntity);

            }
            return BadRequest("User Not Added");
        }
    }
}

