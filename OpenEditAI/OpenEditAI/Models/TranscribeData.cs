using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenEditAI.Models
{
    public class TranscribeData
    {
        public int Id { get; set; }
        public int Seek { get; set; }
        public double Start { get; set; }
        public double End { get; set; }
        public string Text { get; set; }
        public List<int> Tokens { get; set; }
        public double Temperature { get; set; }
        public double AvgLogProb { get; set; }
        public double CompressionRatio { get; set; }
        public double NoSpeechProb { get; set; }
    }
}
