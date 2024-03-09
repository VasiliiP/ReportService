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

    public string Id { get; init; }
    public string Name { get; init; }

    public List<Employee> Employees { get; } = [];
}