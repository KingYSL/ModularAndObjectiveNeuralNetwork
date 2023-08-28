using NeuralNetWorkV2.Network.Enums;
using NeuralNetWorkV2.Network.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Models
{
    public abstract class PerceptronBase:IPerceptron
    {
        public  Guid ID { get; set; }
        public  Network Network { get; protected set; }
        public  float Output { get; protected set; }
        public abstract PerceptronType Type { get; protected set; }
        public List<Connection> Connections { get; set; }
   

        public PerceptronBase(Network network)
        {
            ID = Guid.NewGuid();
            Output = 0;
            Network = network;
            Connections = new List<Connection>();
          
        }

        public abstract void Process(float? inputValue = null);

        public virtual void AddInputConnection(Connection connection)
        {
            Connections.Add(connection);
        }

        public virtual void AddOutputConnection(Connection connection)
        {
            Connections.Add(connection);
        }

        public virtual List<Connection> GetConnections()
        {
            return Connections.ToList();
        }

        public virtual PerceptronBase GetPerceptronFromId(Guid id)
        {
            if (id == ID) return this;

            else
                return null;
        }


        // Other common methods...
    }
}