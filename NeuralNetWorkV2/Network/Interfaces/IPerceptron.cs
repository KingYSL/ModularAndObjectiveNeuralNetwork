using NeuralNetWorkV2.Network.Enums;
using NeuralNetWorkV2.Network.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Interfaces
{
    public interface IPerceptron
    {
        public Guid ID { get; set; }
        public List<Connection> Connections { get;  set; }
   
        Network Network { get; }
        float Output { get; }
        PerceptronType Type { get; }

        void Process(float? inputValue = null);

        public virtual void AddInputConnection(Connection connection)
        { }


        public virtual void AddOutputConnection(Connection connection)
        { }


      
    }
}
