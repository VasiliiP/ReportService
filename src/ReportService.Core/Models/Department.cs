namespace ReportService.Core.Models;

public record Department
{
    public Department(string id, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentException.ThrowIfNullOrEmpty(name);
        Id = id;
        Name = name;
    }

    public string Id { get; set; }
    public string Name { get; set; }

    public List<Employee> Employees { get; } = new();
}