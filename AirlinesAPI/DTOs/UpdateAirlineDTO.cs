using System.ComponentModel.DataAnnotations;

namespace AirlinesAPI.DTOs
{
    public class UpdateAirlineDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public string IATA { get; set; } = null!;

        [Required]
        public string ICAO { get; set; } = null!;
    }
}
