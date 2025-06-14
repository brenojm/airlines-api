using AirlinesAPI.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AirlinesAPI.Services
{
    public class AirlinesService
    {
        private readonly IMongoCollection<Airline> _airlines;
        private readonly AircraftsService _aircrafts;

        public AirlinesService(
            IOptions<AirlinesDatabaseSettings> settings,
            AircraftsService aircraftsService)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _airlines = database.GetCollection<Airline>(settings.Value.AirlinesCollectionName);
            _aircrafts = aircraftsService;
        }

        public Task<List<Airline>> GetAllAsync() =>
            _airlines.Find(_ => true).ToListAsync();

        public async Task<Airline> GetAsync(string id)
        {
            var a = await _airlines.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (a == null) throw new KeyNotFoundException($"Airline {id} not found");
            return a;
        }

        public async Task CreateAsync(Airline airline)
        {
            var exists = await _airlines.Find(x =>
                x.Name == airline.Name ||
                x.IATA == airline.IATA ||
                x.ICAO == airline.ICAO
            ).AnyAsync();
            if (exists) throw new InvalidOperationException("Name, IATA or ICAO already exists");
            await _airlines.InsertOneAsync(airline);
        }

        public async Task UpdateAsync(string id, Airline airline)
        {
            await GetAsync(id);
            var duplicate = await _airlines.Find(x =>
                x.Id != id &&
                (x.Name == airline.Name ||
                 x.IATA == airline.IATA ||
                 x.ICAO == airline.ICAO)
            ).AnyAsync();
            if (duplicate) throw new InvalidOperationException("Name, IATA or ICAO already exists");
            airline.Id = id;
            await _airlines.ReplaceOneAsync(x => x.Id == id, airline);
        }

        public async Task RemoveAsync(string id)
        {
            await GetAsync(id);
            await _airlines.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<Airline>> GetAllWithFleetAsync()
        {
            var list = await GetAllAsync();
            foreach (var a in list)
                a.Fleet = await _aircrafts.GetByAirlineAsync(a.Id!);
            return list;
        }

        public async Task<Airline?> GetWithFleetAsync(string id)
        {
            var a = await GetAsync(id);
            a.Fleet = await _aircrafts.GetByAirlineAsync(id);
            return a;
        }

        public Task<List<Aircraft>> GetFleetAsync(string airlineId) =>
            _aircrafts.GetByAirlineAsync(airlineId);

        public async Task AssignAircraftToFleetAsync(string airlineId, string aircraftId)
        {
            await GetAsync(airlineId);
            var ac = await _aircrafts.GetAsync(aircraftId);
            if (ac.AirlineId == airlineId)
                throw new InvalidOperationException("Aircraft already in fleet");
            ac.AirlineId = airlineId;
            await _aircrafts.UpdateAsync(aircraftId, ac);
        }

        public async Task RemoveAircraftFromFleetAsync(string airlineId, string aircraftId)
        {
            await GetAsync(airlineId);
            var ac = await _aircrafts.GetAsync(aircraftId);
            ac.AirlineId = null;
            await _aircrafts.UpdateAsync(aircraftId, ac);
        }
    }
}
