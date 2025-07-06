using ParseTheParcel.Application.DTOs;

namespace ParseTheParcel.Application.Validators
{
    public static class ParcelRequestValidator
    {
        public static string? Validate(ParcelRequest request)
        {
            if (request.Length <= 0 || request.Breadth <= 0 || request.Height <= 0)
                return "Invalid dimensions. All values must be greater than 0.";

            if (request.Weight <= 0)
                return "Invalid weight. Must be greater than 0.";

            return null; // No validatio error found
        }
    }
}
