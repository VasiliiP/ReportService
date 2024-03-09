namespace ReportService.Core.Dto;

public record EmployeeDto
{
    public string Name { get; set; }
    public string Inn { get; set; }
    public string DepartmentId { get; set; }
    public string DepartmentName { get; set; }
}