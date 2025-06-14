using AirlinesAPI.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AirlinesAPI.Services
{
    public class AircraftsService
    {
        private readonly IMongoCollection<Aircraft> _aircraftCollection;

        public AircraftsService(
            IOptions<AirlinesDatabaseSettings> airlineDatabaseSettings)
        {
            var client = new MongoClient(airlineDatabaseSettings.Value.ConnectionString);
            var db = client.GetDatabase(airlineDatabaseSettings.Value.DatabaseName);
            _aircraftCollection = db.GetCollection<Aircraft>(airlineDatabaseSettings.Value.AircraftsCollectionName);
        }

        public async Task<List<Aircraft>> GetAsync() =>
            await _aircraftCollection.Find(_ => true).ToListAsync();

        public async Task<Aircraft> GetAsync(string id)
        {
            var aircraft = await _aircraftCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (aircraft == null) throw new KeyNotFoundException($"Aircraft {id} not found");
            return aircraft;
        }

        public async Task CreateAsync(Aircraft newAircraft)
        {
            var exists = await _aircraftCollection.Find(x =>
                x.MSN == newAircraft.MSN ||
                x.Registration == newAircraft.Registration
            ).AnyAsync();
            if (exists) throw new InvalidOperationException("MSN or Registration already exists");
            await _aircraftCollection.InsertOneAsync(newAircraft);
        }

        public async Task UpdateAsync(string id, Aircraft updatedAircraft)
        {
            await GetAsync(id);
            var duplicate = await _aircraftCollection.Find(x =>
                x.Id != id &&
                (x.MSN == updatedAircraft.MSN ||
                 x.Registration == updatedAircraft.Registration)
            ).AnyAsync();
            if (duplicate) throw new InvalidOperationException("MSN or Registration already exists");
            updatedAircraft.Id = id;
            await _aircraftCollection.ReplaceOneAsync(x => x.Id == id, updatedAircraft);
        }

        public async Task RemoveAsync(string id)
        {
            await GetAsync(id);
            await _aircraftCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<Aircraft>> GetByAirlineAsync(string airlineId) =>
            await _aircraftCollection.Find(a => a.AirlineId == airlineId).ToListAsync();
    }
}
