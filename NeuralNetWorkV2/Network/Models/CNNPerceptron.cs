using NeuralNetWorkV2.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Models
{
    public class CNNPerceptron : PerceptronBase
    {
        public CNNPerceptron(Network network) : base(network)
        {
        }

        public override PerceptronType Type { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public override void Process(float? inputValue = null)
        {
            throw new NotImplementedException();
        }
    }
}