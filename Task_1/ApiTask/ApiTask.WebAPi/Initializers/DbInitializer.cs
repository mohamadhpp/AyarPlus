using ApiTask.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ApiTask.WebApi.Initializers
{
    public static class DbInitializer
    {
        public static void DBUpdate(ref WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MSSQLContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}