using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Models
{
    // This class aims to model and represent the output of a perceptron at any given time during its training process, it is meant for archiving, record, and data propagation
    public class Output
    {
        public DateTime Time { get; set; }
        public float Value { get; set; }

        public Output(DateTime time, float value)
        {
            Time = time;
            Value = value;
        }
        //default constructor
        public Output() { } 
    }
}