using System.Runtime.CompilerServices;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using ReportService.Core.Abstractions;
using ReportService.Web.Endpoints;
using ReportService.Core.Services;
using ReportService.Infrastructure;
using ReportService.Infrastructure.Config;
using ReportService.Infrastructure.HttpServices;
using ReportService.Web.Middleware;

[assembly: InternalsVisibleTo("ReportService.Tests")]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DbOptions>(builder.Configuration.GetSection("DbOptions"));
builder.Services.Configure<HrApiOptions>(builder.Configuration.GetSection("HrApiOptions"));
builder.Services.Configure<AccountingApiOptions>(builder.Configuration.GetSection("AccountingApiOptions"));

builder.Services.AddScoped<ISalaryReportService, SalaryReportService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IEmployeeFactory, EmployeeFactory>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IValidator<GetSalaryReportRequest>, GetSalaryReportRequestValidator>();
builder.Services.AddLazyCache();

builder.Services.AddHttpClient<IAccountingService, AccountingService>();
builder.Services.AddHttpClient<IHumanResourcesService, HrCachedService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


var app = builder.Build();
app.UseExceptionHandler(opt => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.RegisterReportEndpoints();

app.Run();