using NeuralNetWorkV2.Network.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Network_Data
{
    public class NetworkData
    {
        private static readonly int BITS = 8;
        private static readonly char INVALID_CHAR = '?';

        public List<float> RawValues { get; set; }



        public NetworkData(List<float> rawValues)
        {
            RawValues = Utilities.CloneList(rawValues);
        }
        public NetworkData(List<bool> rawValues)
        {
            RawValues = rawValues.Select(x => x ? 1.0f : 0.0f).ToList();
        }

        public NetworkData(char c)
        {
            RawValues = Utilities.ToBits(Encoding.UTF8.GetBytes(c.ToString())[0], BITS).RawValues.Select(x => x ? 1.0f : 0.0f).ToList();
        }

        public NetworkData(int i)
        {
            RawValues = Utilities.ToBits(i, BITS).RawValues.Select(x => x ? 1.0f : 0.0f).ToList();
        }

        public NetworkData(Utilities.Bits bits)
        {
            RawValues = bits.RawValues.Select(x => x ? 1.0f : 0.0f).ToList();
        }

        public Utilities.Bits BitValues
        {
            get
            {
                return new Utilities.Bits(RawValues.Select(x => Math.Round(x) == 1.0d).ToList());
            }
        }

        public int ParseInt()
        {
            return Utilities.IntFromBits(BitValues);
        }

        public char ParseChar()
        {
            int utfCode = Utilities.IntFromBits(BitValues);
            if (utfCode < byte.MinValue || utfCode > byte.MaxValue)
            {
                return INVALID_CHAR;
            }
            return Encoding.UTF8.GetString(new byte[1] { (byte)utfCode })[0];
        }
    }
}

