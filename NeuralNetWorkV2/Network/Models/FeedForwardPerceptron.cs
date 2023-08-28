using NeuralNetWorkV2.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Models
{
    //This class aims to be a Normal Perceptron component or module, by providing the ability Compute outputs without any specifications
    [Serializable]
    public class FeedForwardPerceptron:PerceptronBase
    {


        public override PerceptronType Type
        {
            get
            {
                if (!Connections.Any(x => x.Out.ID == ID) && Connections.Any(x => x.In.ID == ID))
                    return PerceptronType.Input;
                if (Connections.Any(x => x.Out.ID == ID) && !Connections.Any(x => x.In.ID == ID))
                    return PerceptronType.Output;
                return PerceptronType.Hidden;
                throw new Exception("Perceptron is not properly connected");
            }
            protected set { }
        }

        public FeedForwardPerceptron(Network network):base(network) 
        {
            
            ID = Guid.NewGuid();
            Output = 0;
            Network = network;
        }
        public override void Process(float? inputValue = null)
        {
            if (inputValue != null && Type != PerceptronType.Input && Type != PerceptronType.Hidden)
            {
                throw new ArgumentException();
            }

            float sum = inputValue.HasValue ? inputValue.Value : 0.0f;

            // Process InputConnections
            foreach (Connection connection in Connections)
            {
                if (connection.Out.ID == ID)
                {
                    if (connection.In.Output == 0)
                    {
                        throw new Exception("Previous layer is not fully processed");
                    }
                    sum += connection.In.Output * connection.Weight;
                }
            }

        

            Output = (float)Network.ActivationFunction(sum);
        }


        public override string ToString()
        {
            return string.Format("Feed Forward Perceptron #{0} ({1})", ID, Type);
        }
    }
}
