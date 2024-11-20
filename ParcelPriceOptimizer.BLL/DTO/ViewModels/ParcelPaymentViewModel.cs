using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.BLL.DTO.ViewModels
{
    public class ParcelPaymentViewModel
    {
        public decimal Price { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
        public int Weight { get; set; }
        public decimal Volume => Width * Height * Depth;
        public string? UserId { get; set; }
        public string Courier { get; set; }
    }
}