using System;
using System.Collections.Generic;

namespace SSMKittel.StringDistance
{
    internal class DString : IComparable<DString>, IEquatable<DString>
    {
        public readonly string Value;
        private readonly ulong presence;

        public DString(string value, ulong presence)
        {
            this.Value = value;
            this.presence = presence;
        }

        // Taken from wikipedia, Hamming_weight
        internal static uint popcount64b(ulong x)
        {
            x -= (x >> 1) & 0x5555555555555555UL;
            x = (x & 0x3333333333333333UL) + ((x >> 2) & 0x3333333333333333UL);
            x = (x + (x >> 4)) & 0x0f0f0f0f0f0f0f0fUL;
            x += x >> 8;
            x += x >> 16;
            x += x >> 32;
            return (uint)(x & 0x7f);
        }

        public uint? Distance(DString other, uint threshold)
        {
            if (popcount64b(presence ^ other.presence) > 2 * threshold)
            {
                return null;
            }
            uint edits = Edits(other);
            return edits <= threshold ? (uint?)edits : null;
        }

        // Modified from https://github.com/tdebatty/java-string-similarity/blob/master/src/main/java/info/debatty/java/stringsimilarity/Damerau.java
        internal uint Edits(DString other)
        {
            string s1 = this.Value;
            string s2 = other.Value;
            // INFinite distance is the max possible distance
            int inf = s1.Length + s2.Length;

            // Create and initialize the character array indices
            var da = new Dictionary<char, int>();

            for (int d = 0; d < s1.Length; d++)
            {
                da[s1[d]] = 0;
            }

            for (int d = 0; d < s2.Length; d++)
            {
                da[s2[d]] = 0;
            }

            // Create the distance matrix H[0 .. value.length+1][0 .. other.length+1]
            int[,] h = new int[s1.Length + 2, s2.Length + 2];

            // initialize the left and top edges of H
            for (int i = 0; i <= s1.Length; i++)
            {
                h[i + 1, 0] = inf;
                h[i + 1, 1] = i;
            }

            for (int j = 0; j <= s2.Length; j++)
            {
                h[0, j + 1] = inf;
                h[1, j + 1] = j;
            }

            // fill in the distance matrix H
            // look at each character in value
            for (int i = 1; i <= s1.Length; i++)
            {
                int db = 0;

                // look at each character in b
                for (int j = 1; j <= s2.Length; j++)
                {
                    int i1 = da[s2[j - 1]];
                    int j1 = db;

                    int cost = 1;
                    if (s1[i - 1] == s2[j - 1])
                    {
                        cost = 0;
                        db = j;
                    }

                    h[i + 1, j + 1] = min(
                            h[i, j] + cost, // substitution
                            h[i + 1, j] + 1, // insertion
                            h[i, j + 1] + 1, // deletion
                            h[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1));
                }

                da[s1[i - 1]] = i;
            }

            return (uint) h[s1.Length + 1, s2.Length + 1];
        }

        private static int min(int a, int b, int c, int d)
        {
            return Math.Min(Math.Min(a, b), Math.Min(c, d));
        }

        public int CompareTo(DString other)
        {
            int c = Value.Length.CompareTo(other.Value.Length);
            if (c != 0)
            {
                return c;
            }
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is DString && this == (DString)obj;
        }

        public override int GetHashCode()
        {
            var hashCode = 425489278;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + presence.GetHashCode();
            return hashCode;
        }

        public bool Equals(DString other)
        {
            return this == other;
        }
        public static bool operator ==(DString x, DString y)
        {
            return x.presence == y.presence && x.Value == y.Value;
        }
        public static bool operator !=(DString x, DString y)
        {
            return !(x == y);
        }
    }
}
