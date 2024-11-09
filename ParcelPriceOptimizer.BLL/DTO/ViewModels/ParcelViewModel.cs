﻿using ParcelPriceOptimizer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.BLL.DTO.ViewModels
{
    public class ParcelViewModel
    {
        public int Id { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public decimal Weight { get; set; }
        public decimal Volume => Width * Height * Depth;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CustomerInputId { get; set; }
        public CustomerInput CustomerInput { get; set; }
        public ICollection<ShippingQuote> ShippingQuotes { get; set; }
    }
}
