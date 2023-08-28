using NeuralNetWorkV2.Network.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Network_Data
{
    [Serializable]  
    public class NetworkSaveData
    {
        public int ActivationFunctionType { get; set; }
        public float LearningRate { get; set; }     
        public List<PerceptronSaveData> Inputs { get; set; }
        public List<PerceptronSaveData> Hidden { get; set; }
        public List<PerceptronSaveData> Output { get; set; }
        public List<ConnectionSaveData> Connections { get; set; }

        public int LayerCount { get; set; }

        public List<LayerCreationTypes> LayerArchetichure { get;set; }
    }

    [Serializable]
    public class PerceptronSaveData
    {
        public Guid Id { get; set; }
    }

    [Serializable]
    public class ConnectionSaveData
    {
        public Guid Id { get; set; }
        public Guid InId { get; set; }
        public Guid OutId { get; set; }
        public float Weight { get; set; }
    }
}

