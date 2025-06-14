namespace AirlinesAPI.Entities
{
    public class AirlinesDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string AirlinesCollectionName { get; set; } = null!;

        public string AircraftsCollectionName { get; set; } = null!;
    }
}
