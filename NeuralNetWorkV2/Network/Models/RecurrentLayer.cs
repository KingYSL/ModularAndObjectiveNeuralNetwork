using NeuralNetWorkV2.Network.Enums;
using NeuralNetWorkV2.Network.Network_Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NeuralNetWorkV2.Network.Models
{
    [Serializable]
    public class RecurrentLayer : LayerBase
    {
        public override int LayerCount { get; set; }

        public RecurrentLayer(int inputSize, Network network) : base(inputSize, network) 
        { 
        Network = network;
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
            return new RNNPerceptron(network);
        }

        public  void ConnectLayer(LayerBase previousLayer)
        {
            // Connect each perceptron to every other perceptron in the same layer
            foreach (PerceptronBase currentNeuron in Perceptrons)
            {
                foreach (PerceptronBase otherNeuron in Perceptrons)
                {
                    Connection connection = new Connection(currentNeuron, otherNeuron, GetRandomWeight());
                    currentNeuron.AddOutputConnection(connection);
                    otherNeuron.AddInputConnection(connection);
                }
            }

            ConnectRecurrent();
        }
        public void ConnectRecurrent()
        {
            foreach (PerceptronBase perceptron in Perceptrons)
            {
                // Create recurrent connection
                Connection recurrentConnection = new Connection(perceptron, perceptron,GetRandomWeight());

                // Add recurrent connection to the perceptron
                perceptron.AddOutputConnection(recurrentConnection);
            }
        }
        public virtual void Unfold(int timesteps)
        {
            // Clone this layer for each additional timestep
            for (int i = 0; i < timesteps - 1; i++)
            {
                RecurrentLayer unfoldedLayer = (RecurrentLayer)this.Clone();

                // Connect the unfolded layer to the previous timestep's layer
                unfoldedLayer.ConnectLayer(Network.Layers[Network.Layers.Count - 1]);
                unfoldedLayer.LayerCount=Network.Layers.Count;

                // Add the unfolded layer to the network
                Network.Layers.Add(unfoldedLayer);
            }
        }
      
                
        public override string ToString()
        {
            return string.Format("Recurrent Layer #{0} ({1})", LayerCount, Type);
        }

        private RecurrentLayer Clone()
        {
            return this;
        }
    }
}