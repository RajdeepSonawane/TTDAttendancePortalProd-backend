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
    public class LocationController: Controller


    {

        private readonly ApplicationDbContext dbContext;

        public LocationController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }






        //[Authorize(Roles = "Super Admin")]
        [HttpGet("location-list")]
        public IActionResult GetLocation()
        {
            var locations = dbContext.Locations.ToList();


            return Ok(locations);
        }

        [Authorize]
        [Authorize(Roles = "Super Admin")]
        [HttpPost("add-location")]
        public IActionResult AddLocation( AddLocationDto locationDto)
        {
            
                var locationEntity = new Location()
                {
                    Name = locationDto.Name,
     
                };

                dbContext.Locations.Add(locationEntity);
                dbContext.SaveChanges();

                return Ok(locationEntity);

            
           
        }
    }
}


