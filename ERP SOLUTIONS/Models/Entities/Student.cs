namespace ERP_SOLUTIONS.Models.Entities
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FullName { get; set; } = "";
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string RollNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoin { get; set; }
        public string PermanentAddress { get; set; }
        public string PresentAddress { get; set; }
        public string ParentName { get; set; }
        public string ParentContact { get; set; }
        public bool IsActive { get; set; }
    }



}
