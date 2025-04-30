namespace EmployeeReportApp.Models
{
    public class EmployeeReportDto
    {
        public string FullName { get; set; }
        public string Department { get; set; }
        public string HireDate { get; set; }
        public decimal Salary { get; set; }
        public int YearsOfService { get; set; }
        public decimal Bonus { get; set; }
    }
}
