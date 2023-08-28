// See https://aka.ms/new-console-template for more information
using NeuralNetWorkV2.Network;
using NeuralNetWorkV2.Network.Enums;
using NeuralNetWorkV2.Network.Network_Data;
using NeuralNetWorkV2.Network.Resources;

Console.WriteLine("Hello, World!");
void Main(string[] args)
{ 
    string alphabet = "abcdefghijklmnopqrstuvwxyz";
    /*
   
    List<LayerCreationTypes> activationFunctions = new List<LayerCreationTypes>
    {
        LayerCreationTypes.FeedForward,
                LayerCreationTypes.FeedForward,





        LayerCreationTypes.FeedForward
    };












    List<LearningData> trainingData = new List<LearningData>();
    for (int i = 0; i < alphabet.Length; i++)
    {
        if (i == alphabet.Length - 1)
        {
            trainingData.Add(new LearningData(new NetworkData(alphabet[i]), new NetworkData(i - alphabet.Length)));
        }
        else

            trainingData.Add(new LearningData(new NetworkData(alphabet[i]), new NetworkData(alphabet[i + 1])));
    }


    Network neuralNetwork = new Network(8, activationFunctions,1.0f);
    // Training
    Console.WriteLine("Teaching...");
    int maxProgress = 10;
    int currentProgress = 0;
    neuralNetwork.StartTeaching(trainingData, 1000);
    float progress = neuralNetwork.Teach();
    while (progress < 1.0f)
    {
        while (currentProgress < Math.Round(maxProgress * progress))
        {
            Console.Write("*");
            currentProgress++;
        }
        progress = neuralNetwork.Teach();

    }
    Console.WriteLine(" Ready!");

    // Testing
    bool running = true;
    while (running)
    {
        char keyChar = Console.ReadKey(true).KeyChar.ToString().ToLower()[0];
        if (alphabet.Contains(keyChar))
        {
            NetworkData output = neuralNetwork.Process(new NetworkData(keyChar));
            Console.WriteLine(string.Format("{0} -> {1}", keyChar, output.ParseChar()));
        }
        else
        {
            running = false;
        }
    }

    neuralNetwork.Save(neuralNetwork);
    */
    
    var ntwk = Network.Load(@$");

    bool running = true;
    while (running)
    {
        char keyChar = Console.ReadKey(true).KeyChar.ToString().ToLower()[0];
        if (alphabet.Contains(keyChar))
        {
            NetworkData output = ntwk.Process(new NetworkData(keyChar));
            Console.WriteLine(string.Format("{0} -> {1}", keyChar, output.ParseChar()));
        }
        else
        {
            running = false;
        }
    }
    

}
Main(args);