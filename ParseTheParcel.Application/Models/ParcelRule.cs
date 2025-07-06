namespace ParseTheParcel.Application.Models
{
    public class ParcelRule
    {
        public string Type { get; set; } = string.Empty;
        public int MaxLength { get; set; }
        public int MaxBreadth { get; set; }
        public int MaxHeight { get; set; }
        public double MaxWeight { get; set; }  
        public double Cost { get; set; }
    }
}
