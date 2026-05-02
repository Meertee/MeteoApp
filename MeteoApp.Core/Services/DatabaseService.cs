using MeteoApp.Core.Interfaces;
using MeteoApp.Core.Models;
using SQLite;

namespace MeteoApp.Core.Services
{
    public class DatabaseService : IDatabaseService
    {
        private SQLiteAsyncConnection? _database;
        private readonly string _dbPath;
        
        public DatabaseService()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _dbPath = Path.Combine(basePath, "MeteoApp.db3");
        }

        public async Task InitializeAsync()
        {
            if (_database != null) return;
            string key = "SuperSecretKey";
            var options = new SQLiteConnectionString(_dbPath, true, key:  key);
            _database = new SQLiteAsyncConnection(options);
            await _database.CreateTableAsync<WeatherLocation>();
        }

        public async Task<int> DeleteLocationAsync(WeatherLocation location)
        {
            await InitializeAsync();
            return await _database!.DeleteAsync(location);
        }

        public async Task<List<WeatherLocation>> GetAllLocationsAsync()
        {
            await InitializeAsync();
            return await _database!.Table<WeatherLocation>().ToListAsync();
        }

        public async Task<WeatherLocation> GetCurrentLocationAsync()
        {
            await InitializeAsync();
            return await _database!.Table<WeatherLocation>().Where(i => i.IsCurrentLocation).FirstOrDefaultAsync();
        }

        public async Task<WeatherLocation> GetLocationAsync(int id)
        {
            await InitializeAsync();
            return await _database!.Table<WeatherLocation>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveLocationAsync(WeatherLocation location)
        {
            await InitializeAsync();
            try
            {
                if (location.IsCurrentLocation)
                {
                    var existingCurrentLocation = await GetCurrentLocationAsync();
                    if (existingCurrentLocation != null && existingCurrentLocation.Id != location.Id)
                    {
                        existingCurrentLocation.IsCurrentLocation = false;
                        await _database!.UpdateAsync(existingCurrentLocation);
                    }
                }
                if (location.Id != 0)
                {
                    return await _database!.UpdateAsync(location);
                }
                else
                {
                    var existing = await _database!.Table<WeatherLocation>()
                            .Where(l => l.CityName == location.CityName)
                            .FirstOrDefaultAsync();

                    if (existing != null)
                    {
                        location.Id = existing.Id;
                        return await _database!.UpdateAsync(location);
                    }
                    else
                    {
                        return await _database!.InsertAsync(location);
                    }
                }
            }
            catch (SQLiteException ex) when (ex.Message.Contains("Constraint") || ex.Message.Contains("UNIQUE"))
            {
                System.Diagnostics.Debug.WriteLine($"Duplicate entry blocked: {location.CityName}");
                throw new InvalidOperationException($"The city '{location.CityName}' is already saved.");
            }
        }
    }
}
