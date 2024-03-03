using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEditAI.Models
{
    public class Segment
    {
        public Double Start { get; set; } = 0;
        public Double Duration { get; set; } = 0;

        public override string ToString()
        {
            return $"Start: {Start}, Duration: {Duration}";
        }
    }
}
