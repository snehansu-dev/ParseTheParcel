namespace ParseTheParcel.Domain.ValueObjects;

public record Dimensions(long Length, long Breadth, long Height)
{
    public bool IsValid() =>
        Length > 0 && Breadth > 0 && Height > 0;

    public override string ToString() =>
        $"{Length}mm x {Breadth}mm x {Height}mm";
}
