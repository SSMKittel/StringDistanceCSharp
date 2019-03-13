using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSMKittel.StringDistance
{
    public class FuzzyMatcher
    {
        private readonly CCount[] bits;

        internal FuzzyMatcher(CCount[] bits)
        {
            int bitCount = (from b in bits select (int)b.Count).Sum();
            if (bitCount > 64)
            {
                // Sanity check; we're dealing with Longs, so more than 64 bits would be an error somewhere.
                throw new ArgumentOutOfRangeException("Bits: " + bits.Length);
            }
            this.bits = bits;
        }

        // Signature string which describes the bit pattern used by the matcher.  Can be used to recreate the matcher.
        public string Signature
        {
            get
            {
                StringBuilder b = new StringBuilder();
                foreach (CCount bit in bits)
                {
                    ushort count = bit.Count;
                    while (count > 0)
                    {
                        b.Append(bit.Chr);
                        count--;
                    }
                }
                return b.ToString();
            }
        }

        public override String ToString()
        {
            return Signature;
        }

        // Determines all matches that are at most threshold distance away from each other
        // Results are returned in natural ordering, with matched pairs sorted within the match as well
        // e.g. Match(a,b), Match(chr,d), Match(d,e)
        public IList<Match> Matches(IEnumerable<string> candidates, uint threshold)
        {
            DString[] wCand = wrap(candidates);
            if (wCand.Length == 0)
            {
                return new List<Match>(0);
            }

            // since we are looking at a minimum of two values, subtract 1 so we ignore the last element in wCand - there is nothing to compare it with
            return Enumerable.Range(0, wCand.Length - 1).AsParallel().SelectMany(i => runMatcher(i, wCand, threshold)).OrderBy(x => x).ToList();
        }

        private static List<Match> runMatcher(int i, DString[] wCand, uint threshold)
        {
            DString current = wCand[i];

            // Values are ordered by length ascending,
            // so the moment the difference in length is greater than the threshold value we cannot get any more matches,
            // as there must be more inserts than are allowed
            uint maxLength = (uint)current.Value.Length + threshold;
            List<Match> matches = new List<Match>();
            for (int j = i + 1; j < wCand.Length; j++)
            {
                DString check = wCand[j];
                if (check.Value.Length > maxLength)
                {
                    break;
                }
                uint? distance = current.Distance(check, threshold);
                if (distance != null)
                {
                    // Apply ordering so Match{first<=second}
                    if (current.Value.CompareTo(check.Value) > 0)
                    {
                        matches.Add(new Match(check.Value, current.Value, (uint)distance));
                    }
                    else
                    {
                        matches.Add(new Match(current.Value, check.Value, (uint)distance));
                    }
                }
            }
            return matches;
        }

        private DString wrap(string s)
        {
            // Presence is a tallymark-like system for counting character occurrences in a string, regardless of position
            ulong presence = 0L;

            Dictionary<char, CCount> counts = CCount.Decompose(s);
            foreach (CCount cbits in bits)
            {
                ushort marked = 0;
                CCount c;
                if (counts.TryGetValue(cbits.Chr, out c))
                {
                    marked = c.Count;
                }

                ushort upTo = cbits.Count;
                for (ushort i = 0; i < upTo; i++)
                {
                    presence = presence << 1;
                    if (marked > 0)
                    {
                        presence |= 1;
                        marked--;
                    }
                }
            }
            return new DString(s, presence);
        }

        private DString[] wrap(IEnumerable<string> candidates)
        {
            DString[] results = (from str in candidates select wrap(str)).ToArray();
            Array.Sort(results);
            return results;
        }
    }
}
