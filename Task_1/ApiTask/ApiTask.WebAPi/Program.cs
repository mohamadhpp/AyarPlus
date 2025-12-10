using ApiTask.Application.Interfaces.Repositories;
using ApiTask.Application.Interfaces.Repositories.LocationRepositories;
using ApiTask.Application.Mapper;
using ApiTask.Infrastructure.Persistence.Contexts;
using ApiTask.Infrastructure.Persistence.Repositories;
using ApiTask.Infrastructure.Persistence.Repositories.LocationRepositories;
using ApiTask.Infrastructure.Persistence.Repositories.UserRepositories;
using ApiTask.WebApi.Initializers;

var builder = WebApplication.CreateBuilder(args);

#region Builder

#region Configuration

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#if DEBUG
    .AddJsonFile("appsettings.development.json", optional: true)
#else
    .AddJsonFile("appsettings.production.json", optional: true)
#endif
    .AddEnvironmentVariables();

var configuration = builder.Configuration;

#endregion

#region Init Application

// Init folders
FolderInitializer.InitFolders();

#endregion

#region Database Contexts

builder.Services.AddDbContext<MSSQLContext>();

builder.Services.AddSingleton<ILoggerFactory>(provider => LoggerFactory.Create(builder =>
{
    builder.AddConsole();
}));

#endregion

#region Repositories

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();

#endregion

#region Auto Mapper

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserMapperProfile>();
    cfg.AddProfile<ContactMapperProfile>();
    cfg.AddProfile<CompanyMapperProfile>();
}, typeof(Program).Assembly);

#endregion

#region Request

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

#endregion

builder.Services.AddControllers();
builder.Services.AddOpenApi();

#endregion

var app = builder.Build();

#region App

#region Database Update

// Init database 
DbInitializer.DBUpdate(ref app);

// Data import
DataInitializer.LocationData(ref app);

#endregion

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.Run();

#endregion