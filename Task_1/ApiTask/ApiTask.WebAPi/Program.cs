using ApiTask.Application.Interfaces.Repositories;
using ApiTask.Application.Interfaces.Repositories.LocationRepositories;
using ApiTask.Application.Mapper;
using ApiTask.Infrastructure.Persistence.Contexts;
using ApiTask.Infrastructure.Persistence.Repositories;
using ApiTask.Infrastructure.Persistence.Repositories.LocationRepositories;
using ApiTask.Infrastructure.Persistence.Repositories.UserRepositories;
using ApiTask.WebApi.Initializers;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

#region Builder

#region Configuration

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builder.Configuration;

#endregion

#region Init Application

// Init folders
FolderInitializer.InitFolders();

#endregion

#region Database Contexts

builder.Services.AddDbContext<MSSQLContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("MSSQLServer"));
});

builder.Services.AddSingleton<ILoggerFactory>(provider => LoggerFactory.Create(builder =>
{
    builder.AddConsole();
}));

#endregion

#region Repositories

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();

#endregion

#region Auto Mapper

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserMapperProfile>();
    cfg.AddProfile<ContractMapperProfile>();
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

#region Swagger

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "رابط برنامه‌نویسی کاربردی ApiTask",
        Version = "ورژن 1",
        Description = "API برای مدیریت کاربران، شرکت‌ها، قراردادها"
    });

    c.EnableAnnotations();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

#endregion

#endregion

var app = builder.Build();

#region App

#region Database Update

// Init database 
DbInitializer.DBUpdate(ref app);

// Data import
DataInitializer.LocationData(ref app);

#endregion

#region Swagger

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiTask API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "ApiTask API";

        // Styles
        c.InjectStylesheet("/fonts/iransans/font.css");
        c.InjectStylesheet("/plugins/custom/swagger/swagger.css");

        // Scripts
        c.InjectJavascript("/plugins/custom/swagger/swagger.js");
    });
}

#endregion

app.UseCors();
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

// Launch Swagger in browser
if (app.Environment.IsDevelopment())
{
    var url = app.Urls.FirstOrDefault() ?? "http://localhost:5045";
    var swaggerUrl = $"{url}/swagger";

    _ = Task.Run(() =>
    {
        Thread.Sleep(1000);

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = swaggerUrl,
            UseShellExecute = true
        });
    });
}

app.Run();

#endregion