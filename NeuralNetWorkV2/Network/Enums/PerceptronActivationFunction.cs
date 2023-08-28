using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Enums
{
    public class PerceptronActivationFunctions
    {
        public static float Sigmoid(double value)
        {
            return (float)(1.0 / (1.0 + Math.Pow(Math.E, -value)));
        }

        public static float SigmoidDerivate(float value)
        {
            return value * (1.0f - value);
        }
    }
}
