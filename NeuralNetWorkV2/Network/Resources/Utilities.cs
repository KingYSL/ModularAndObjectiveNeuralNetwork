using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetWorkV2.Network.Resources
{
    public class Utilities
    {
        public static List<T> CloneList<T>(List<T> list)
        {
            List<T> clone = new List<T>();
            foreach (T item in list)
            {
                clone.Add(item);
            }
            return clone;
        }
        public static List<T?>? CloneLists<T>(List<T?> list)
        {
            List<T?> clone = new List<T?>();
            foreach (T? item in list)
            {
                clone.Add(item);
            }
            return clone;
        }

        public static Bits ToBits(int integer, int bitsCount)
        {
            string bits = Convert.ToString(integer, 2).PadLeft(bitsCount, '0');
            if (bits.Length > bitsCount)
            {
                bits = bits.Substring(bits.Length - bitsCount);
            }
            List<bool> list = new List<bool>();
            for (int i = 0; i < bits.Length; i++)
            {
                list.Add(bits[i] == '1');
            }
            return new Bits(list);
        }

        public static int IntFromBits(Bits bits)
        {
            StringBuilder builder = new StringBuilder();
            foreach (bool bit in bits.RawValues)
            {
                builder.Append(bit ? "1" : "0");
            }
            return Convert.ToInt32(builder.ToString(), 2);
        }
        public static float[][] EncodeStringOneHot(string txt, out List<char> dict)
        {
            dict = new List<char>();
            for (int i = 0; i < txt.Length; i++)
            {
                if (!dict.Contains(txt[i]))
                {
                    dict.Add(txt[i]);
                }
            }

            float[][] output = new float[txt.Length][];
            for (int i = 0; i < txt.Length; i++)
            {
                output[i] = new float[dict.Count];
                Fill(output[i], 0.0f);
                output[i][dict.IndexOf(txt[i])] = 1.0f;
            }
            return output;
        }

        public static void Fill(float[] f, float v)
        {
            int i = f.Length;
            while (i-- > 0)
            {
                f[i] = v;
            }
        }
        public class Bits : IEnumerable<bool>
        {
            public List<bool> RawValues { get; private set; }

            public Bits(List<bool> rawValues)
            {
                RawValues = Utilities.CloneList(rawValues);
            }

            public Bits(string s)
            {
                RawValues = new List<bool>();
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '1')
                    {
                        RawValues.Add(true);
                    }
                    else if (s[i] == '0')
                    {
                        RawValues.Add(false);
                    }
                    else
                    {
                        throw new ArgumentException("String must only contain ones and zeros");
                    }
                }
            }

            public IEnumerator<bool> GetEnumerator()
            {
                return RawValues.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return RawValues.GetEnumerator();
            }

            public bool this[int index]
            {
                get
                {
                    return RawValues[index];
                }
                set
                {
                    RawValues[index] = value;
                }
            }

            public override string ToString()
            {
                return new string(RawValues.Select(x => x ? '1' : '0').ToArray());
            }

        }


    }
}
