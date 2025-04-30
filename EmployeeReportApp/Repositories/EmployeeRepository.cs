using EmployeeReportApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeReportApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<EmployeeReportDto>> GetEmployeeReportDataAsync()
        {
            var employees = new List<EmployeeReportDto>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_GetEmployeeReportData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(new EmployeeReportDto
                            {
                                FullName = reader["FullName"].ToString(),
                                Department = reader["Department"].ToString(),
                                HireDate = reader["FormattedHireDate"].ToString(),
                                Salary = Convert.ToDecimal(reader["Salary"]),
                                YearsOfService = Convert.ToInt32(reader["YearsOfService"]),
                                Bonus = Convert.ToDecimal(reader["Bonus"])
                            });
                        }
                    }
                }
            }

            return employees;
        }
    }
    }
