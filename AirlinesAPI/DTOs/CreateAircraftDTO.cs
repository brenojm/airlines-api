using System.ComponentModel.DataAnnotations;

namespace AirlinesAPI.DTOs
{
    public class CreateAircraftDTO
    {
        [Required]
        [StringLength(50)]
        public string MSN { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Registration { get; set; } = null!;
    }
}
