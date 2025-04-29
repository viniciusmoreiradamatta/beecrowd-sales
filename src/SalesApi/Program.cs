using Prometheus;
using SalesApi.Configuration;
using SalesApi.Endpoints;
using SalesApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

await builder.AddSalesDependencies();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();

app.UseHttpMetrics();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();
});

await app.RunAsync();