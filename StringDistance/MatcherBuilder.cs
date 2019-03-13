using System;
using System.Collections.Generic;

namespace SSMKittel.StringDistance
{
    public class MatcherBuilder
    {
        private uint total = 0;
        private Dictionary<char, List<uint>> counts = new Dictionary<char, List<uint>>();

        public static FuzzyMatcher Matcher(string signature)
        {
            if (signature.Length > 64)
            {
                throw new ArgumentException("Signature too large");
            }
            // Special logic isn't needed; as long as it fits within 64 bits,
            // the normal algorithm will reconstruct the bit pattern without errors
            MatcherBuilder matcherBuilder = new MatcherBuilder();
            matcherBuilder.Update(signature);
            return matcherBuilder.Build();
        }

        // Convenience method for just creating a matcher from a set of data.
        public static FuzzyMatcher Matcher(IEnumerable<string> strs)
        {
            MatcherBuilder matcherBuilder = new MatcherBuilder();
            matcherBuilder.Update(strs);
            return matcherBuilder.Build();
        }

        public void Update(IEnumerable<string> strs)
        {
            foreach (string str in strs)
            {
                Update(str);
            }
        }

        public void Update(string str)
        {
            if (str.Length == 0)
            {
                return;
            }
            foreach (CCount cc in CCount.Decompose(str).Values)
            {
                List<uint> vals;
                if (!counts.TryGetValue(cc.Chr, out vals))
                {
                    vals = new List<uint>();
                    counts.Add(cc.Chr, vals);
                }
                while (vals.Count < cc.Count)
                {
                    // Insert zeros until the array of counts can support the value we want to insert
                    vals.Add(0);
                }
                // We can't have a count of 0, so count positions need to be offset by 1
                vals[cc.Count - 1]++;
            }
            total++;
        }

        public FuzzyMatcher Build()
        {
            Dictionary<char, Queue<uint>> costs = computeCosts();

            // Setup a priority queue in largest-first order, with the first cost value for each character
            var q = new SortedSet<WeightedChar>();
            foreach (KeyValuePair<char, Queue<uint>> e in costs)
            {
                q.Add(new WeightedChar(e.Key, e.Value.Dequeue()));
            }

            var bitCounts = new Dictionary<char, ushort>();
            int iterations = 64; // bits in a Long
            while (iterations > 0 && q.Count != 0)
            {
                WeightedChar wc = q.Max;
                q.Remove(wc);

                // Increment bit count for the character
                ushort bitsCurrent;
                if (!bitCounts.TryGetValue(wc.Chr, out bitsCurrent))
                {
                    bitsCurrent = 0;
                }
                bitsCurrent++;
                bitCounts[wc.Chr] = bitsCurrent;
                Queue<uint> vals = costs[wc.Chr];
                if (vals != null && vals.Count != 0)
                {
                    // Costs queue still has values to go through, put it back into the priority queue
                    q.Add(new WeightedChar(wc.Chr, vals.Dequeue()));
                }
                iterations--;
            }

            // Flatten the bitCounts map into an array of (character, bits)
            CCount[] bits = new CCount[bitCounts.Count];
            int i = 0;
            foreach (KeyValuePair<char, ushort> e in bitCounts)
            {
                bits[i] = new CCount(e.Key, e.Value);
                i++;
            }
            Array.Sort(bits);
            return new FuzzyMatcher(bits);
        }

        private Dictionary<char, Queue<uint>> computeCosts()
        {
            var allCosts = new Dictionary<char, Queue<uint>>();
            foreach (KeyValuePair<char, List<uint>> e in counts)
            {
                char ch = e.Key;
                List<uint> count = e.Value;

                // Including the total number of strings used to compute the weights simplifies the cost calculation
                uint[] cost = new uint[1 + count.Count];
                cost[0] = total;
                int i = 1;
                foreach (uint countVal in count)
                {
                    cost[i] = countVal;
                    i++;
                }

                // Back-fill the array so we are using running totals
                // e.g. when a character appears 4 times in a string, also say that it appears 3, 2, and 1 times in the same string
                // This also means that cost[i] >= cost[i + 1]
                for (i = cost.Length - 1; i > 1; i--)
                {
                    cost[i - 1] = cost[i - 1] + cost[i];
                }

                // Simple algorithm to determine cost values, by only looking at one character on its own
                // A character that never appears in any of our strings is just as useless as a character that always appears
                // A better choice is characters which appear 50% of the time, as it can cut out half of the values we are comparing
                // Similarly, a choice which halves 4 to 2 is less useful than one which halves 100 to 50
                // So the cost is determined by multiplying the size of the subset of words that it pertains to,
                // with a fraction determined by how close the reduction is to half the subset's size
                for (i = cost.Length - 1; i > 1; i--)
                {
                    uint previous = cost[i - 1];
                    uint halfpoint = previous / 2; // Rounding is irrelevant
                    uint current = cost[i];
                    if (current > halfpoint)
                    {
                        // Drop it below the half point so it has roughly the same distance to the halfpoint, but is smaller than it
                        current = previous - current;
                    }
                    double ranking = current / (double)halfpoint;
                    cost[i] = (uint)Math.Round(ranking * previous);
                }

                var costQ = new Queue<uint>(cost.Length - 1);
                // First element in the cost array was used for setup only and is ignored
                for (i = 1; i < cost.Length; i++)
                {
                    costQ.Enqueue(cost[i]);
                }
                allCosts.Add(ch, costQ);
            }
            return allCosts;
        }
    }
}
