using NeuralNetWorkV2.Network.Enums;
using NeuralNetWorkV2.Network.Models;
using NeuralNetWorkV2.Network.Network_Data;
using NeuralNetWorkV2.Network.Resources;
using Newtonsoft.Json;

namespace NeuralNetWorkV2.Network
{

    public class Network
    {
        public List<LayerBase> Layers;


        public NetworkActivationFunctionType ActivationType;
        private List<LearningData> learningData;
        private float learningRate;
        private int learningDataRepeats;
        private int learningDataCycle;

        public Network(int inputSize, List<LayerCreationTypes> layerTypes, float Learningrate)
        {
            Layers = new List<LayerBase>();
            learningRate = Learningrate;


            // Create hidden layers based on layerTypes
            for (int i = 0; i < layerTypes.Count; i++)
            {
                if (layerTypes[i] == LayerCreationTypes.FeedForward)
                {
                    Layers.Add(new FeedForwardLayer(inputSize, this));
                    Layers[i].LayerCount = i;
                }
                else if (layerTypes[i] == LayerCreationTypes.Recurrent)
                {
                    Layers.Add(new RecurrentLayer(inputSize, this));

                    Layers[i].LayerCount = i;

                }
                else
                    throw new ArgumentException("Invalid layer type");
            }




            ConnectLayers();

        }



        private void ConnectLayers()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                LayerBase previousLayer = Layers[i - 1];
                LayerBase currentLayer = Layers[i];

                foreach (PerceptronBase currentPerceptron in currentLayer.Perceptrons)
                {
                    Random rng = new Random();
                    foreach (PerceptronBase previousPerceptron in previousLayer.Perceptrons)
                    {
                        Connection connection = new Connection(previousPerceptron, currentPerceptron, rng.Next(100) * 0.01f);
                        currentPerceptron.AddInputConnection(connection);
                    }
                }
            }
        }


        public void StartTeaching(List<LearningData> data, int repeats)
        {
            if (repeats <= 0)
            {
                throw new ArgumentException("Invalid number of repeats");
            }
            learningData = data;
            learningDataRepeats = repeats;
            learningDataCycle = 0;
        }
        public float Teach()
        {
            if (learningData == null || learningDataRepeats <= 0)
            {
                throw new Exception("StartTeaching needs to be called before parameterless Teach is called");
            }
            if (learningDataCycle >= learningDataRepeats)
            {
                return 1.0f;
            }
            foreach (LearningData data in learningData)
            {
                Teach(data.Input.RawValues, data.ExpectedOutput.RawValues);
            }
            learningDataCycle++;
            TestResults results = Analytics.Test(this, learningData);
            Console.WriteLine(string.Format("Success rate: {0}% ({1}/{2})", (int)(results.SuccessRate * 100.0f), results.SuccessfulTests, results.TotalTests));
            return learningDataCycle / (float)learningDataRepeats;
        }

        public NetworkData Teach(List<float> inputValues, List<float> expectedOutputs)
        {
            if (inputValues.Count != Layers[0].Perceptrons.Count)
            {
                throw new ArgumentException(string.Format("Invalid number of input values, {0} was given while {1} expected", inputValues.Count, Layers[0].Perceptrons.Count));
            }
            if (expectedOutputs.Count != Layers.Last().Perceptrons.Count)
            {
                throw new ArgumentException(string.Format("Invalid number of output values, {0} was given while {1} expected", expectedOutputs.Count, Layers.Last().Perceptrons.Count));
            }

            List<float> results = Process(inputValues).RawValues;

            float totalError = 0.0f;
            float delta = 0.0f;
            for (int i = 0; i < expectedOutputs.Count; i++)
            {
                totalError += (0.5f * (float)Math.Pow(expectedOutputs[i] - results[i], 2.0d));
                delta += (results[i] - expectedOutputs[i]);
            }
            if (totalError == 0.0f)
            {
                return new NetworkData(results);
            }

            // Calculate errors
            Dictionary<PerceptronBase, float> errors = new Dictionary<PerceptronBase, float>();
            for (int i = 0; i < Layers.Last().Perceptrons.Count; i++)
            {
                float error = (expectedOutputs[i] - Layers.Last().Perceptrons[i].Output) * ActivationFunctionDerivate(Layers.Last().Perceptrons[i].Output);
                errors.Add(Layers.Last().Perceptrons[i], error);
            }

            for (int layerIndex = Layers.Count - 1; layerIndex >= 1; layerIndex--)
            {
                List<PerceptronBase> layer = Layers[layerIndex].Perceptrons;
                foreach (PerceptronBase perceptron in layer)
                {
                    float error = 0.0f;
                    foreach (Connection connection in perceptron.Connections)
                    {
                        if (connection.Out.ID != perceptron.ID)
                        {
                            error += connection.Weight * errors[connection.Out];
                        }
                    }
                    if (!errors.ContainsKey(perceptron))
                        errors.Add(perceptron, error * ActivationFunctionDerivate(perceptron.Output));
                }
            }

            // Update weights
            foreach (LayerBase layer in Layers)
            {
                foreach (PerceptronBase perceptron in layer.Perceptrons)
                {
                    foreach (Connection connection in perceptron.Connections)
                    {
                        connection.Weight += learningRate * errors[connection.Out] * connection.In.Output;
                    }
                }
            }

            return new NetworkData(results);
        }



        public NetworkData Process(List<float> inputValues)
        {
            if (inputValues.Count != Layers[0].Perceptrons.Count)
            {
                throw new ArgumentException(string.Format("Invalid number of input values, {0} was given while {1} expected", inputValues.Count, Layers[0].Perceptrons.Count));
            }
            foreach (float inputValue in inputValues)
            {
                if (inputValue < 0.0f || inputValue > 1.0f)
                {
                    throw new ArgumentException(string.Format("Invalid input value {0}, value must be between 0.0 and 1.0", inputValue));
                }
            }

            for (int i = 0; i < inputValues.Count; i++)
            {
                Layers[0].Perceptrons[i].Process(inputValues[i]);
            }
            //pass the sum of the hidden layer to the next layer
            for (int layerIndex = 1; layerIndex < Layers.Count; layerIndex++)
            {
                List<PerceptronBase> perceptrons = Layers[layerIndex].Perceptrons;
                foreach (PerceptronBase perceptron in perceptrons)
                {
                    perceptron.Process();
                }
            }

            List<float> result = new List<float>();
            foreach (PerceptronBase perceptron in Layers.Last().Perceptrons)
            {
                perceptron.Process();
                result.Add(perceptron.Output);
            }

            return new NetworkData(result);
        }

        public NetworkData Process(NetworkData input)
        {
            return Process(input.RawValues);
        }




        public float ActivationFunction(float input)
        {
            switch (ActivationType)
            {
                case NetworkActivationFunctionType.Sigmoid:
                    return PerceptronActivationFunctions.Sigmoid((double)input);
                default:
                    throw new Exception(string.Format("ActivationFunctionType {0} is missing implementation", ActivationType));
            }
        }

        public float ActivationFunctionDerivate(float input)
        {
            switch (ActivationType)
            {
                case NetworkActivationFunctionType.Sigmoid:
                    return PerceptronActivationFunctions.SigmoidDerivate(input);
                default:
                    throw new Exception(string.Format("ActivationFunctionType {0} is missing implementation", ActivationType));
            }
        }



        public List<float> FeedForward(List<float> inputs)
        {
            if (inputs.Count != Layers[0].Perceptrons.Count)
            {
                throw new ArgumentException("Input size does not match the network configuration.");
            }

            List<float> outputs = inputs;
            foreach (FeedForwardPerceptron perceptron in Layers[0].Perceptrons)
            {
                perceptron.Process();
            }

            for (int i = 1; i < Layers.Count; i++)
            {
                foreach (FeedForwardPerceptron perceptron in Layers[i].Perceptrons)
                {
                    perceptron.Process();
                }
                outputs = Layers[i].GetOutputs();
            }

            return outputs;
        }


        public void Save(Network network)
        {
            string baseDirect = @"";

            if (Directory.Exists(baseDirect + DateTime.Now.ToShortDateString()))
            {
                NetworkSaveData data = new NetworkSaveData()
                {
                    ActivationFunctionType = (int)network.ActivationType,
                    LearningRate = network.learningRate,
                    Connections = new List<ConnectionSaveData>(),
                    Hidden = new List<PerceptronSaveData>(),
                    Inputs = network.Layers.First().Perceptrons.Select(x => new PerceptronSaveData() { Id = x.ID }).ToList(),
                    LayerArchetichure = new List<LayerCreationTypes>(),




                    Output = network.Layers.Last().Perceptrons.Select(x => new PerceptronSaveData() { Id = x.ID }).ToList(),

                };
                //adding hidding layers
                foreach (var lay in network.Layers)
                {
                    if (lay is FeedForwardLayer)
                    {
                        data.LayerArchetichure.Add(LayerCreationTypes.FeedForward);
                    }

                    else if (lay is RecurrentLayer)
                    {
                        data.LayerArchetichure.Add(LayerCreationTypes.Recurrent);
                    }

                    if (lay.Type == LayerType.Hidden)
                    {
                        data.Hidden.AddRange(lay.Perceptrons.Select(x => new PerceptronSaveData() { Id = x.ID }).ToList());
                    }
                }

                foreach (var layer in network.Layers)
                {
                    foreach (var per in layer.Perceptrons)
                    {
                        var connections = new List<ConnectionSaveData>();
                        var connect = per.GetConnections();
                        connections.AddRange(connect.Select(x => new ConnectionSaveData() { Id = x.Id, InId = x.In.ID, OutId = x.Out.ID, Weight = x.Weight }));
                        data.Connections.AddRange(connections);
                    }
                }

                var jsonString = JsonConvert.SerializeObject(data
                     , Formatting.Indented, new JsonSerializerSettings()
                     {


                     });

                var filename = $@"{baseDirect}{DateTime.Now.ToShortDateString()}\{data}.Json";
                File.WriteAllText(filename, jsonString);
            }
            else
            {

                CreateDirectory(DateTime.Now.ToShortDateString()).Wait();
            }

        }
        public PerceptronBase GetPerceptronFromId(Guid id)
        {
            // Validate that the Layers collection is not null or empty
            if (Layers == null || !Layers.Any())
            {
                Console.WriteLine("The Layers collection is null or empty.");
                throw new InvalidOperationException("The Layers collection is null or empty.");
            }

            try
            {
                // Search for the Perceptron with the specified ID in all layers
                foreach (var layer in Layers)
                {
                    // Validate that the Perceptrons collection in the layer is not null or empty
                    if (layer?.Perceptrons == null || !layer.Perceptrons.Any())
                    {
                        Console.WriteLine($"The Perceptrons collection in layer {layer} is null or empty.");
                        continue; // Skip to the next layer
                    }

                    // Search for the Perceptron with the specified ID in the current layer
                    foreach (var perceptron in layer.Perceptrons)
                    {
                        var foundPerceptron = perceptron.GetPerceptronFromId(id);
                        if (foundPerceptron != null)
                        {
                            return foundPerceptron; // Perceptron found, return it
                        }
                    }
                }

                // If code reaches here, the Perceptron was not found
                Console.WriteLine($"Perceptron with the ID {id} was not found.");
                throw new ArgumentOutOfRangeException($"Perceptron with the ID {id} was not found.");
            }
            catch (Exception ex) // Catch all exceptions
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Re-throw the caught exception
            }
        }

        public async Task CreateDirectory(string FileName)
        {
            string baseDirect = @"" + FileName;

            if (!Directory.Exists(baseDirect))
            {
                Directory.CreateDirectory(baseDirect);
            }

        }
        public static Network Load(string file)
        {
            try
            {
                using (StreamReader r = new StreamReader(file))
                {
                    var NET = JsonConvert.DeserializeObject<NetworkSaveData>(File.ReadAllText(file));

                    // Create the network with the saved architecture
                    Network network = new Network(NET.Inputs.Count, NET.LayerArchetichure, NET.LearningRate);

                    // Create a dictionary to map IDs to perceptrons for quick lookup
                    Dictionary<Guid, PerceptronBase> idToPerceptron = new Dictionary<Guid, PerceptronBase>();

                    // Assign IDs for Input layer perceptrons
                    for (int i = 0; i < NET.Inputs.Count; i++)
                    {
                        network.Layers[0].Perceptrons[i].ID = NET.Inputs[i].Id;
                    }

                    // Assign IDs for Output layer perceptrons
                    for (int i = 0; i < NET.Output.Count; i++)
                    {
                        network.Layers.Last().Perceptrons[i].ID = NET.Output[i].Id;
                    }

                    // Assign IDs for Hidden layer perceptrons
                    // Assume NET.Hidden is a flat list, and you have to distribute them across hidden layers
                    int hiddenIndex = 0;
                    for (int layerIndex = 1; layerIndex < network.Layers.Count - 1; layerIndex++) // Skipping input and output layers
                    {
                        for (int perIndex = 0; perIndex < network.Layers[layerIndex].Perceptrons.Count; perIndex++)
                        {
                            network.Layers[layerIndex].Perceptrons[perIndex].ID = NET.Hidden[hiddenIndex].Id;
                            hiddenIndex++;
                        }
                    }


                    foreach (var layer in network.Layers)
                    {
                        foreach (var perceptron in layer.Perceptrons)
                        {
                            if (perceptron.ID != Guid.Empty)
                            {
                                idToPerceptron[perceptron.ID] = perceptron;
                                Console.WriteLine($"Added {perceptron.ID} to dictionary.");
                            }
                            else
                            {
                                Console.WriteLine($"Perceptron has empty ID.");
                            }
                        }
                    }

                    // Debug output to show dictionary size
                    Console.WriteLine($"Dictionary contains {idToPerceptron.Count} items.");

                    // Rebuild the connections based on saved data
                    foreach (var connData in NET.Connections)
                    {
                        if (idToPerceptron.TryGetValue(connData.InId, out var inPerceptron))
                        {
                            // Successfully found the inPerceptron
                            Console.WriteLine($"Found inPerceptron with ID {connData.InId}.");
                        }
                        else
                        {
                            // Failed to find the inPerceptron
                            Console.WriteLine($"Failed to find inPerceptron with ID {connData.InId}.");
                            continue;  // Skip this iteration
                        }

                        if (idToPerceptron.TryGetValue(connData.OutId, out var outPerceptron))
                        {
                            // Successfully found the outPerceptron
                            Console.WriteLine($"Found outPerceptron with ID {connData.OutId}.");
                        }
                        else
                        {
                            // Failed to find the outPerceptron
                            Console.WriteLine($"Failed to find outPerceptron with ID {connData.OutId}.");
                            continue;  // Skip this iteration
                        }

                        // If both TryGetValue calls are successful, proceed to create the connection
                        var newConnection = new Connection(inPerceptron, outPerceptron, connData.Weight);
                        inPerceptron.Connections.Add(newConnection);
                        outPerceptron.Connections.Add(newConnection);
                    }


                    return network;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the network: {ex.Message}");
                throw;
            }
        }

    }
}


