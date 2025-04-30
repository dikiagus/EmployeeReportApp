using EmployeeReportApp.Models;

namespace EmployeeReportApp.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeReportDto>> GetEmployeeReportDataAsync();
    }
}
