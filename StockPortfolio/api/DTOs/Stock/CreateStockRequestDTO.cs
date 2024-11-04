using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Stock
{
    public class CreateStockRequestDTO
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol lenght must be under 10 characters ")]
        public string Symbol { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100, ErrorMessage = "CompanyName lenght must be under 10 characters ")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot be over 10 characters")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(1, 5000000000)]
        public long MarketCap { get; set; }
    }
}