using System;
using System.Collections.Generic;

namespace SSMKittel.StringDistance
{
    internal struct CCount : IComparable<CCount>
    {
        public readonly char Chr;
        public readonly ushort Count;

        public CCount(char chr, ushort count)
        {
            this.Chr = chr;
            this.Count = count;
        }

        public static Dictionary<char, CCount> Decompose(string str)
        {
            var m = new Dictionary<char, ushort>();
            foreach (char chr in str)
            {
                ushort val;
                if (!m.TryGetValue(chr, out val))
                {
                    val = 0;
                }
                val++;
                m[chr] = val;
            }

            var results = new Dictionary<char, CCount>();
            foreach (KeyValuePair<char, ushort> e in m)
            {
                results.Add(e.Key, new CCount(e.Key, e.Value));
            }
            return results;
        }

        public int CompareTo(CCount other)
        {
            int c = Chr.CompareTo(other.Chr);
            if (c != 0)
            {
                return c;
            }
            return Count.CompareTo(other.Count);
        }
    }
}
