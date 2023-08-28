using NeuralNetWorkV2.Network.Enums;
using NeuralNetWorkV2.Network.Interfaces;
using NeuralNetWorkV2.Network.Network_Data;
using NeuralNetWorkV2.Network.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Models
{
    public abstract class LayerBase: ILayer
    {
        [Newtonsoft.Json.JsonIgnore]
        public Network Network { get; set; }
        public int LayerConnectionCount => Perceptrons.Sum(x => x.Connections.Count);

        public List<PerceptronBase> Perceptrons { get; protected set; }
        public abstract LayerType Type { get; }
        public abstract int LayerCount { get; set; }

        public LayerBase(int size, Network network)
        {
            Perceptrons = new List<PerceptronBase>();
            for (int i = 0; i < size; i++)
            {
                Perceptrons.Add(CreatePerceptron(network));
            }
        }

        protected abstract PerceptronBase CreatePerceptron(Network network);

        public void AddPerceptron(PerceptronBase perceptron)
        {
            Perceptrons.Add(perceptron);
        }

        public virtual void ConnectLayer(LayerBase previousLayer)
        {
            foreach (PerceptronBase currentPerceptron in Perceptrons)
            {
                foreach (PerceptronBase previousPerceptron in previousLayer.Perceptrons)
                {
                    Connection connection = new Connection(previousPerceptron, currentPerceptron, GetRandomWeight());
                    currentPerceptron.AddInputConnection(connection);
                    
                }
            }
        }
        public virtual void Backpropagate(double[] errors)
        {
            // For each perceptron in the layer
            for (int i = 0; i < Perceptrons.Count; i++)
            {
                // Calculate the error signal
                var error = errors[i] * Network.ActivationFunctionDerivate(Perceptrons[i].Output);

                // Update each weight of the perceptron
                for (int j = 0; j < Perceptrons[i].Connections.Count; j++)
                {
                    // Weight update rule: Δw = η * δ * x
                    Perceptrons[i].Connections[j].Weight =  (float)( error * Perceptrons[i].Connections[j].Weight);
                }
            }
        }
        public virtual void CreateRecurrentConnections()
        {
            foreach (PerceptronBase perceptron in Perceptrons)
            {
                foreach (PerceptronBase otherPerceptron in Perceptrons)
                {
                    if (perceptron != otherPerceptron)
                    {
                        Connection connection = new Connection(perceptron, otherPerceptron, GetRandomWeight());
                        perceptron.AddOutputConnection(connection);
                        otherPerceptron.AddInputConnection(connection);
                    }
                }
            }
        }
        public virtual void ConnectToOutputLayer(LayerBase outputLayer)
        {
            foreach (PerceptronBase perceptron in Perceptrons)
            {
                foreach (PerceptronBase outputPerceptron in outputLayer.Perceptrons)
                {
                    Connection connection = new Connection(perceptron, outputPerceptron, GetRandomWeight());
                    perceptron.AddOutputConnection(connection);
                    outputPerceptron.AddInputConnection(connection);
                }
            }
        }
        public virtual float GetRandomWeight()
        {
            Random rng = new Random();
            return rng.Next(100) * 0.01f;
        }

        public void Process()
        {
            foreach (PerceptronBase neuron in Perceptrons)
            {
                neuron.Process();
            }
        }

        public List<float> GetOutputs()
        {
            return Perceptrons.Select(p => p.Output).ToList();
        }

        public override string ToString()
        {
            return string.Format("Layer #{0} ({1})", LayerCount, Type);
        }
    }
}