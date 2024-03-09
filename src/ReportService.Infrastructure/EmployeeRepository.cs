using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using ReportService.Core.Abstractions;
using ReportService.Core.Dto;
using ReportService.Infrastructure.Config;

namespace ReportService.Infrastructure;

public class EmployeeRepository : IEmployeeRepository
{
    public EmployeeRepository(IOptions<DbOptions> options)
    {
        ArgumentException.ThrowIfNullOrEmpty(options.Value.ConnectionString);
        _connectionString = options.Value.ConnectionString;
    }

    private readonly string _connectionString;

    private NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public async Task<IReadOnlyCollection<EmployeeDto>> GetAllEmployees()
    {
        const string sql = """
                           SELECT e.name As name, e.inn AS inn, d.name AS departmentName, d.id as departmentId
                           FROM emps e
                           INNER JOIN deps d on e.departmentid = d.id
                           """;
        await using var connection = CreateConnection();
        var employees = await connection.QueryAsync<EmployeeDto>(sql);
        return employees.ToList();
    }
}