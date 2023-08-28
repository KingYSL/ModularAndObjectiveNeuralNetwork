using NeuralNetWorkV2.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Models
{
    [Serializable]
    public class FeedForwardLayer : LayerBase
    {
        public override int LayerCount { get; set; } 

        public FeedForwardLayer(int inputSize, Network network) : base(inputSize, network) 
        {
            Network=network;
        }
        public override LayerType Type
        {
            get
            {
                if (Perceptrons.All(p => p.Type == PerceptronType.Input))
                {
                    return LayerType.Input;
                }
                else if (Perceptrons.All(p => p.Type == PerceptronType.Output))
                {
                    return LayerType.Output;
                }
                return LayerType.Hidden;
            }
        }

        protected override PerceptronBase CreatePerceptron(Network network)
        {
            return new FeedForwardPerceptron(network);
        }

        public  void ConnectLayer(LayerBase previousLayer)
        {
            foreach (PerceptronBase currentNeuron in Perceptrons)
            {
                foreach (PerceptronBase previousNeuron in previousLayer.Perceptrons)
                {
                    Connection connection = new Connection(previousNeuron, currentNeuron, GetRandomWeight());
                    currentNeuron.AddInputConnection(connection);
                   
                }
            }

        
        }
    

        public override string ToString()
        {
            return string.Format("Feed Forward Layer #{0} ({1})", LayerCount, Type);
        }
    }
}