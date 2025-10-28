using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Models
{
    public class CurrencyRate
    {
        public DateTime Date { get; set; }
        public decimal Rate { get; set; }
        public string Currency { get; set; }
        public decimal ConvertedAmount { get; set; }
    }
}
