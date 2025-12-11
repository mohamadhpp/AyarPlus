using ApiTask.Domain.Entities.Location;
using ApiTask.Infrastructure.Persistence.Contexts;
using System.Text.Json;

namespace ApiTask.WebApi.Initializers
{
    public static class DataInitializer
    {
        #region Loacation Data

        public static void LocationData(ref WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MSSQLContext>();

                string ProvincesCitiesJsonPath = Path.Combine(AppContext.BaseDirectory,
                                                              "wwwroot",
                                                              "data",
                                                              "provinces_cities.json");

                if (!System.IO.File.Exists(ProvincesCitiesJsonPath))
                {
                    return;
                }

                #region Check & Add Country

                Country? country = dbContext.Countries.FirstOrDefault(c => c.Code == "+98");
                if (country == null)
                {
                    country = new Country
                    {
                        Code = "+98",
                        Currency = "ریال",
                        Name = "ایران"
                    };
                    dbContext.Countries.Add(country);
                    dbContext.SaveChanges();
                }

                #endregion

                #region Add Provinces & Cities

                // Load data
                string jsonData = System.IO.File.ReadAllText(ProvincesCitiesJsonPath);
                var locationData = JsonSerializer.Deserialize<LocationData>(jsonData);

                if (locationData?.Provinces != null)
                {
                    foreach (var ProvinceData in locationData.Provinces)
                    {
                        var province = dbContext.Provinces
                            .FirstOrDefault(c => c.Name == ProvinceData.Name);

                        if (province == null)
                        {
                            province = new Province
                            {
                                Name = ProvinceData.Name,
                                Code = ProvinceData.Code,
                                CountryId = country.Id
                            };
                            dbContext.Provinces.Add(province);
                        }
                    }

                    // Save provinces
                    dbContext.SaveChanges();

                    // Add cities
                    foreach (var provinceData in locationData.Provinces)
                    {
                        var province = dbContext.Provinces
                            .FirstOrDefault(p => p.Code == provinceData.Code);

                        if (province != null && provinceData.Cities != null)
                        {
                            foreach (var CityData in provinceData.Cities)
                            {
                                var city = dbContext.Cities
                                    .FirstOrDefault(c => c.ProvinceId == province.Id && c.Name == CityData.Name);

                                if (city == null)
                                {
                                    city = new City
                                    {
                                        ProvinceId = province.Id,
                                        Name = CityData.Name
                                    };
                                    dbContext.Cities.Add(city);
                                }
                            }

                            // Save cities
                            dbContext.SaveChanges();
                        }
                    }
                }

                #endregion
            }
        }

        #endregion
    }

    #region Internal Models

    internal class LocationData
    {
        public List<Province> Provinces { get; set; } = new();
    }

    #endregion
}