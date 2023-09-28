namespace CRUD_Operations_Using_Area.Areas.Student.Models
{
    public class StudentModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; } = string.Empty;
        
        public string MobileNoStudent { get; set;} = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string MobileNoFather { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public String BirthDate { get; set; } = string.Empty;

        public int? Age { get; set; } 

        public bool IsActive { get; set; } = true;

        public string Gender { get; set; } = "Male";

        public string Password { get; set; } = string.Empty;

        public int? BranchID { get; set; }

        public int? CityID { get; set; }

    }
}
