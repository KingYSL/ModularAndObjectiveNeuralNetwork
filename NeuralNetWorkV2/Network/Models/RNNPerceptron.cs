using NeuralNetWorkV2.Network.Enums;
using NeuralNetWorkV2.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Models
{
    [Serializable]
    public class RNNPerceptron : PerceptronBase
    {
        // Properties for the neuron and its connections in the network
     
        public float State { get; protected set; }
        public Queue<Output> PastOutputs { get; private set; }

        // Properties for the LSTM gates and their weights
        private float ForgetGate { get; set; }
        private float OutputGate { get; set; }
        private float InputGate { get; set; }
        private float ForgetGateWeight { get; set; }
        private float OutputGateWeight { get; set; }
        private float InputGateWeight { get; set; }


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

        // Constructor initializes the neuron and its weights
        public RNNPerceptron(Network network) : base(network)
        {
            PastOutputs = new Queue<Output>(100);
            var rand = new Random();
            ForgetGateWeight = (float)rand.NextDouble();
            OutputGateWeight = (float)rand.NextDouble();
            InputGateWeight = (float)rand.NextDouble();
           
            
        }
        // Process the input through the neuron and its gates
        public override void Process(float? inputValue = null)
        {
            if (inputValue != null && Type != PerceptronType.Input && Type != PerceptronType.Hidden)
            {
                throw new ArgumentException("Input value should be null for non-input and non-hidden neurons.");
            }

            // Calculate the weighted sum of the inputs
            float sum = inputValue.HasValue ? inputValue.Value : 0.0f;
            foreach (Connection connection in Connections)
            {
                if (connection.Out.ID == ID && connection.In.Output != 0)
                {
                    sum += connection.In.Output * connection.Weight;
                }
            }

            // Process the inputs through the gates
            ForgetGate = CalculateGateOutput(inputValue, ForgetGateWeight);
            InputGate = CalculateGateOutput(inputValue, InputGateWeight);
            OutputGate = CalculateGateOutput(inputValue, OutputGateWeight);

            // Update the state and output of the neuron
            float candidateCellState = (float)Math.Tanh(sum);
            State = State * ForgetGate + candidateCellState * InputGate;
            Output = OutputGate * (float)Math.Tanh(State);

            // Enqueue the output
            if (PastOutputs.Count == 100)
            {
                PastOutputs.Dequeue();
            }
            PastOutputs.Enqueue(new Models.Output(DateTime.Now, Output));
        }

        // Calculate the output of a gate
        private float CalculateGateOutput(float? inputValue, float gateWeight)
        {
            float sum = inputValue.HasValue ? inputValue.Value : 0.0f;
            foreach (Connection connection in Connections)
            {
                if (connection.Out.ID == ID && connection.In.Output != 0)
                {
                    sum += connection.In.Output * gateWeight;
                }
            }
            return Network.ActivationFunction(sum);
        }
       
        public override string ToString()
        {
            return string.Format("Neuron #{0} ({1})", ID, Type);
        }

    }

}
