using ParseTheParcel.Application.DTOs;

namespace ParseTheParcel.Application.Interfaces
{
    public interface IParcelRuleEvaluator
    {
        (string? Type, double? Cost, string? Message) Evaluate(ParcelRequest request);
    }
}
