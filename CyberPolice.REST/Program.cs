using CyberPolice.Infrastructure;
using CyberPolice.Infrastructure.Models;
using CyberPolice.REST;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "Вставте accessToken, отриманий з /api/auth/login"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// База даних
builder.Services.AddDbContext<CyberPoliceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CyberPoliceDb")));

// Identity: реєстрація, логін, Bearer-токени, ролі
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CyberPoliceContext>();

builder.Services.AddAuthorization();

// Dependency Injection
builder.Services.AddScoped<IRepository<CyberCaseModel>, Repository<CyberCaseModel>>();
builder.Services.AddScoped<IRepository<InvestigatorModel>, Repository<InvestigatorModel>>();
builder.Services.AddScoped<ICrudServiceAsyncDb<CyberCaseModel>, CrudServiceDb<CyberCaseModel>>();
builder.Services.AddScoped<ICrudServiceAsyncDb<InvestigatorModel>, CrudServiceDb<InvestigatorModel>>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await SeedData.InitializeAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api/auth").MapIdentityApi<ApplicationUser>();
app.MapControllers();

app.Run();