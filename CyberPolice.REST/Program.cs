using System.Data.Common;
using CyberPolice.Infrastructure;
using CyberPolice.Infrastructure.Models;

DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CyberPoliceContext>();
builder.Services.AddScoped<IRepository<CyberCaseModel>, Repository<CyberCaseModel>>();
builder.Services.AddScoped<IRepository<InvestigatorModel>, Repository<InvestigatorModel>>();
builder.Services.AddScoped<ICrudServiceAsyncDb<CyberCaseModel>, CrudServiceDb<CyberCaseModel>>();
builder.Services.AddScoped<ICrudServiceAsyncDb<InvestigatorModel>, CrudServiceDb<InvestigatorModel>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();