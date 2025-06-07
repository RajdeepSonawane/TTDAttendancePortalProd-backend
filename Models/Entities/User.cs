namespace TTDAttendancePortal_backend.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        //public required byte[]? EmployeeImage { get; set; }

        public required string EmployeeCode { get; set; }

        public required string FirstName { get; set; }
        public required string MiddleName { get; set; }

        public required string LastName { get; set; }

        public required string ContactNumber { get; set; }
        public required string CurrentAddress { get; set; }


        public required string Email { get; set; }


        public required string Password { get; set; }

        public required string Designation { get; set; }
        public required string Role { get; set; }

        public required string BloodGroup { get; set; }

        public required decimal YearOfExp {  get; set; }
       

        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
