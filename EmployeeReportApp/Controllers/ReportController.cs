using EmployeeReportApp.Repositories;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace EmployeeReportApp.Controllers
{
    // Controllers/ReportController.cs
    public class ReportController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public ReportController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var reportData = await _employeeRepository.GetEmployeeReportDataAsync();
            return View(reportData);
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var reportData = await _employeeRepository.GetEmployeeReportDataAsync();

            OfficeOpenXml.ExcelPackage.License.SetNonCommercialPersonal("Diki Agus");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Employee Report");

                // Header
                worksheet.Cells[1, 1].Value = "Full Name";
                worksheet.Cells[1, 2].Value = "Department";
                worksheet.Cells[1, 3].Value = "Hire Date";
                worksheet.Cells[1, 4].Value = "Salary";
                worksheet.Cells[1, 5].Value = "Years of Service";
                worksheet.Cells[1, 6].Value = "Bonus";

                // Data
                for (int i = 0; i < reportData.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = reportData[i].FullName;
                    worksheet.Cells[i + 2, 2].Value = reportData[i].Department;
                    worksheet.Cells[i + 2, 3].Value = reportData[i].HireDate;
                    worksheet.Cells[i + 2, 4].Value = reportData[i].Salary;
                    worksheet.Cells[i + 2, 5].Value = reportData[i].YearsOfService;
                    worksheet.Cells[i + 2, 6].Value = reportData[i].Bonus;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeReport.xlsx");
            }
        }

        public async Task<IActionResult> ExportToPDF()
        {
            var reportData = await _employeeRepository.GetEmployeeReportDataAsync();

            using (var memoryStream = new MemoryStream())
            {
                var document = new Document();
                var writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();

                // Judul
                document.Add(new Paragraph("Employee Report"));

                // Tabel
                var table = new PdfPTable(6);
                table.AddCell("Full Name");
                table.AddCell("Department");
                table.AddCell("Hire Date");
                table.AddCell("Salary");
                table.AddCell("Years of Service");
                table.AddCell("Bonus");

                foreach (var item in reportData)
                {
                    table.AddCell(item.FullName);
                    table.AddCell(item.Department);
                    table.AddCell(item.HireDate);
                    table.AddCell(item.Salary.ToString("C"));
                    table.AddCell(item.YearsOfService.ToString());
                    table.AddCell(item.Bonus.ToString("C"));
                }

                document.Add(table);
                document.Close();

                return File(memoryStream.ToArray(), "application/pdf", "EmployeeReport.pdf");
            }
        }
    }
}
