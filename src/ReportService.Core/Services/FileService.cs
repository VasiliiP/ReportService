using ReportService.Core.Abstractions;
using ReportService.Core.Models;

namespace ReportService.Core.Services;

public class FileService : IFileService
{
    public byte[] CreateSalaryReport(List<Department> departments, string period)
    {
        using var memoryStream = new MemoryStream();
        using var textWriter = new StreamWriter(memoryStream);

        textWriter.WriteLine(period);
        foreach (var department in departments)
        {
            textWriter.WriteLine("--------------------------------------------");
            textWriter.WriteLine(department.Name);
            foreach (var employee in department.Employees)
            {
                textWriter.WriteLine($"    {employee.Name} {employee.Salary}р");
            }
        }
        textWriter.Flush();
        memoryStream.Position = 0;

        return memoryStream.ToArray();
    }
}