using ParseTheParcel.Domain.ValueObjects;

namespace ParseTheParcel.Domain.Entities;

public class Parcel(Dimensions dimensions, double weight, double maxAllowedWeight)
{
    public Dimensions Dimensions { get; } = dimensions;
    public double Weight { get; } = weight;
    public double MaxAllowedWeight { get; } = maxAllowedWeight;
    public bool IsValid() => Weight <= MaxAllowedWeight;
}
