using System.ComponentModel.DataAnnotations;

namespace ParseTheParcel.Application.DTOs
{
    public record ParcelRequest
    {
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Length must be greater than 0.")]
        public long Length { get; init; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Breadth must be greater than 0.")]
        public long Breadth { get; init; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Height must be greater than 0.")]
        public long Height { get; init; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
        public double Weight { get; init; }
    }



}
