namespace AirlinesAPI.DTOs
{
    public class AircraftDTO
    {
        public string Id { get; set; } = null!;
        public string MSN { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Registration { get; set; } = null!;
        public string? AirlineName { get; set; }
    }
}
