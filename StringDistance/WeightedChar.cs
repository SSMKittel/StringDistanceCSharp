using System;

namespace SSMKittel.StringDistance
{
    internal struct WeightedChar : IComparable<WeightedChar>
    {
        public readonly char Chr;
        public readonly uint Weight;

        public WeightedChar(char chr, uint weight)
        {
            this.Chr = chr;
            this.Weight = weight;
        }

        public int CompareTo(WeightedChar other)
        {
            int c = Weight.CompareTo(other.Weight);
            if (c != 0)
            {
                return c;
            }
            return Chr.CompareTo(other.Chr);
        }
    }
}
