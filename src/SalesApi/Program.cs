using Microsoft.EntityFrameworkCore;
using SalesDomain.Interfaces.Repository;
using SalesDomain.Interfaces.UnitOfWork;
using SalesInfrastructure.Data;
using SalesInfrastructure.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SalesApiDb"), c => c.MigrationsAssembly("SalesInfrastructure")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.RunAsync();