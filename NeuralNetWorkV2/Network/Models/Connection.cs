using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Models
{
    public class Connection
    {
        public Guid Id { get;  set; }
        public PerceptronBase In { get;  set; }
        public PerceptronBase Out { get;  set; }
        public float Weight { get; set; }

        public Connection(PerceptronBase inP, PerceptronBase outP, float weight)
        {
            Id = Guid.NewGuid();

            In = inP;
            Out = outP;
            In.Connections.Add(this);
           
            Weight = weight;
        }


        public float GetRandomWeight()
        {
            Random random = new Random();
            return random.Next() * 2 - 1; // Random weight between -1 and 1
        }

        public float GetWeightedInput()
        {
            return In.Output * Weight;
        }



    }
}
