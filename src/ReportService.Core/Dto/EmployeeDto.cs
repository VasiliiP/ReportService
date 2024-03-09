namespace ReportService.Core.Dto;

public record EmployeeDto
{
    public EmployeeDto(string name, string inn, string departmentId, string departmentName)
    {
        Name = name;
        Inn = inn;
        DepartmentId = departmentId;
        DepartmentName = departmentName;
    }

    public string Name { get; set; }
    public string Inn { get; set; }
    public string DepartmentId { get; set; }
    public string DepartmentName { get; set; }
}