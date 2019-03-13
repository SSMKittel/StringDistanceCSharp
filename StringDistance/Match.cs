using System;
using System.Collections.Generic;

namespace SSMKittel.StringDistance
{
    public struct Match : IEquatable<Match>, IComparable<Match>
    {
        public readonly string First;
        public readonly string Second;
        public readonly uint Distance;

        public Match(string first, string second, uint distance)
        {
            this.First = first;
            this.Second = second;
            this.Distance = distance;
        }

        public int CompareTo(Match other)
        {
            int c = First.CompareTo(other.First);
            if (c != 0)
            {
                return c;
            }
            c = Second.CompareTo(other.Second);
            if (c != 0)
            {
                return c;
            }
            return Distance.CompareTo(other.Distance);
        }

        public override bool Equals(object obj)
        {
            return obj is Match && this == (Match)obj;
        }

        public override int GetHashCode()
        {
            var hashCode = 1099035000;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(First);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Second);
            hashCode = hashCode * -1521134295 + Distance.GetHashCode();
            return hashCode;
        }

        public bool Equals(Match other)
        {
            return this == other;
        }
        public static bool operator ==(Match x, Match y)
        {
            return x.Distance == y.Distance && x.First == y.First && x.Second == y.Second;
        }
        public static bool operator !=(Match x, Match y)
        {
            return !(x == y);
        }
    }
}
