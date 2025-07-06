using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTheParcel.Application.DTOs
{
    public class ParcelResponse
    {
        public string? ParcelType { get; set; }
        public double? Cost { get; set; }
        public string? Message { get; set; }
    }
}
