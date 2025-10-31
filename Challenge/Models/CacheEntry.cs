using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Models
{
    public class CacheEntry
    {
        public object Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
