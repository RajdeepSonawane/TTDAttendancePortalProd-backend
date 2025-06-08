using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TTDAttendancePortal_backend.Data;
using TTDAttendancePortal_backend.Models.Dto;
using Microsoft.EntityFrameworkCore;


namespace TTDAttendancePortal_backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;

        public LoginController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(LoginDto loginDto)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user != null)
            {

              

                bool isMatch = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);

                if (!isMatch)
                {
                    return Unauthorized(new { message = "Invalid password" });

                }

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.EmployeeCode.ToString()),
                    new Claim("Email", user.Email.ToString()),
                    new Claim("FirstName", user.FirstName.ToString()),
                    new Claim("LastName", user.LastName.ToString()),
                    new Claim("Role", user.Role.ToString()),
                    new Claim("Id", user.Id.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"] ?? ""));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signIn);

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = tokenValue });
            }
            return NotFound(new { message = "User not found" });

        }
    }
};

