# Neural Network Framework in C#
A highly optimized Neural Network framework with support for Feed Forward Perceptrons and Recurrent Perceptrons (incomplete).

# Features
1. Feed Forward Perceptrons
1. Recurrent Perceptrons (WIP)
1. Customizable activation functions
1. Layered architecture
1. High-level API for training and inference
# Installation
1. Git Clone
1. Or Download
# Usage
Here is a simple example to create a feed-forward neural network:

```csharp
 List<LayerCreationTypes> layers = new List<LayerCreationTypes>
    {
        LayerCreationTypes.FeedForward,
        LayerCreationTypes.FeedForward,
        LayerCreationTypes.FeedForward
    };
Network network = new Network(inputCount, layers, learningRate);
// Training and inference code here
```

1. Inside the Network Class, in order to save or load network configurations you must ensure the BaseDirectory property inside of the respective methods is correct for your own systems needs.
# Assumptions
The network is initialized with a specified architecture and learning rate.
Feed-forward functionality is complete, while recurrent perceptron functionality is a work-in-progress.
# Known Limitations
Recurrent Perceptron functionality to unfold while being trained is not completely implemented.

# To do
1. Vector arrays (structs)
2. optimize performance
3. implemnent RNN unit functionality
4. Make default environment the Desktop
5. Code Clean Up
# License
This project is open-source, under the MIT license.

