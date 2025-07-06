using ParseTheParcel.Application.DTOs;

namespace ParseTheParcel.Application.Interfaces
{
    public interface IParcelService
    {
        (string? Type, double? Cost, string? Message) GetParcelTypeAndCost(ParcelRequest request);
    }
}
