using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Network_Data
{
    public class LearningData
    {
        public NetworkData Input { get; set; }
        public NetworkData ExpectedOutput { get; set; }

        public LearningData(NetworkData input, NetworkData expectedOutput)
        {
            Input = input;
            ExpectedOutput = expectedOutput;
        }
    }
}
