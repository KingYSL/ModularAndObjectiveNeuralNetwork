using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Network_Data
{
    public class TestResults
    {
        public int SuccessfulTests { get; set; }
        public int FailedTests { get; set; }
        public int TotalTests { get { return SuccessfulTests + FailedTests; } }
        public float SuccessRate { get { return TotalTests == 0 ? 0.0f : SuccessfulTests / (float)TotalTests; } }
    }
}

