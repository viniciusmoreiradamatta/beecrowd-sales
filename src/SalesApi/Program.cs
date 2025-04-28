using Prometheus;
using SalesApi.Configuration;
using SalesApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

await builder.AddSalesDependencies();

var app = builder.Build();

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