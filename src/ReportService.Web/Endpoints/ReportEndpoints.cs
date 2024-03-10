using FluentValidation;
using ReportService.Core.Abstractions;

namespace ReportService.Web.Endpoints;

public static class ReportEndpoints
{
    public static void RegisterReportEndpoints(this WebApplication app)
    {
        app.MapGet("/report/{year:int}/{month:int}", async Task<IResult> (int year, int month,
                ISalaryReportService reportService, IValidator<GetSalaryReportRequest> validator, CancellationToken ct) =>
            {
                validator.ValidateAndThrow(new GetSalaryReportRequest(year, month));
                var report = await reportService.GenerateSalaryReport(year, month, ct);
                return Results.File(report, "application/octet-stream", $"SalaryReport-{year}-{month}.txt");
            })
            .WithName("GetSalaryReport")
            .WithOpenApi();
    }
}

public record GetSalaryReportRequest(int Year, int Month)
{
    public int Year { get; } = Year;
    public int Month { get; } = Month;
}

public class GetSalaryReportRequestValidator : AbstractValidator<GetSalaryReportRequest>
{
    public GetSalaryReportRequestValidator()
    {
        RuleFor(x => x.Month).GreaterThan(0).LessThanOrEqualTo(12);
        RuleFor(x => x.Year).GreaterThan(0);
    }
}