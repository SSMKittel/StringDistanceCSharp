using NUnit.Framework;
using System.Collections.Generic;

namespace SSMKittel.StringDistance.Tests
{
    public class FuzzyMatcherTest
    {
        private FuzzyMatcher matcher;
        private FuzzyMatcher matcherEmpty;
        private readonly IEnumerable<string> words = new string[] {
                "RAINSTORM",
                "BRAINSTORM",
                "BRAINSTORMS",
                "EGOIST",
                "ENLIST",
                "AGONIST",
                "EGOTIST",
                "EGRETS",
                "EXERTS",
                "GREETS",
                "FERRETS",
                "RECEIVER"
        };

        [OneTimeSetUp]
        public void Init()
        {
            matcher = MatcherBuilder.Matcher("AAABBCCCDDDEEEEFFGGGHHIIIIJKKLLLMMNNNOOOPPQRRRSSSSTTTUUVVWWXYYZZ");
            matcherEmpty = MatcherBuilder.Matcher(new string[0]);
        }

        [Test]
        public void WordsMatch1()
        {
            IList<Match> matches = matcher.Matches(words, 1);
            Assert.AreEqual(new Match[] {
                    new Match("BRAINSTORM", "BRAINSTORMS", 1),
                    new Match("BRAINSTORM", "RAINSTORM", 1),
                    new Match("EGOIST", "EGOTIST", 1)
            },
            matches, "Matches");
        }

        [Test]
        public void WordsMatch2()
        {
            IList<Match> matches = matcher.Matches(words, 2);
            Assert.AreEqual(new Match[] {
                    new Match("AGONIST", "EGOIST", 2),
                    new Match("AGONIST", "EGOTIST", 2),
                    new Match("BRAINSTORM", "BRAINSTORMS", 1),
                    new Match("BRAINSTORM", "RAINSTORM", 1),
                    new Match("BRAINSTORMS", "RAINSTORM", 2),
                    new Match("EGOIST", "EGOTIST", 1),
                    new Match("EGOIST", "ENLIST", 2),
                    new Match("EGRETS", "EXERTS", 2),
                    new Match("EGRETS", "FERRETS", 2),
                    new Match("EGRETS", "GREETS", 2)
            },
            matches, "Matches");
        }


        [Test]
        public void WordsMatchEmpty1()
        {
            IList<Match> matches = matcherEmpty.Matches(words, 1);
            Assert.AreEqual(new Match[] {
                    new Match("BRAINSTORM", "BRAINSTORMS", 1),
                    new Match("BRAINSTORM", "RAINSTORM", 1),
                    new Match("EGOIST", "EGOTIST", 1)
            },
            matches, "Matches");
        }

        [Test]
        public void WordsMatchEmpty2()
        {
            IList<Match> matches = matcherEmpty.Matches(words, 2);
            Assert.AreEqual(new Match[] {
                    new Match("AGONIST", "EGOIST", 2),
                    new Match("AGONIST", "EGOTIST", 2),
                    new Match("BRAINSTORM", "BRAINSTORMS", 1),
                    new Match("BRAINSTORM", "RAINSTORM", 1),
                    new Match("BRAINSTORMS", "RAINSTORM", 2),
                    new Match("EGOIST", "EGOTIST", 1),
                    new Match("EGOIST", "ENLIST", 2),
                    new Match("EGRETS", "EXERTS", 2),
                    new Match("EGRETS", "FERRETS", 2),
                    new Match("EGRETS", "GREETS", 2)
            },
            matches, "Matches");
        }

        [Test]
        public void EdgeMatch0()
        {
            IList<Match> matches = matcher.Matches(new string[0], 2);
            Assert.AreEqual(new Match[0], matches, "Matches");
        }

        [Test]
        public void EdgeMatch1()
        {
            IList<Match> matches = matcher.Matches(new string[] { "A" }, 2);
            Assert.AreEqual(new Match[0], matches, "Matches");
        }

        [Test]
        public void EdgeMatch2()
        {
            IList<Match> matches = matcher.Matches(new string[] { "A", "A" }, 2);
            Assert.AreEqual(new Match[] {
                    new Match("A", "A", 0)
            },
            matches, "Matches");
        }

        [Test]
        public void EdgeMatchEmpty0()
        {
            IList<Match> matches = matcherEmpty.Matches(new string[0], 2);
            Assert.AreEqual(new Match[0], matches, "Matches");
        }

        [Test]
        public void EdgeMatchEmpty1()
        {
            IList<Match> matches = matcherEmpty.Matches(new string[] { "A" }, 2);
            Assert.AreEqual(new Match[0], matches, "Matches");
        }

        [Test]
        public void EdgeMatchEmpty2()
        {
            IList<Match> matches = matcherEmpty.Matches(new string[] { "A", "A" }, 2);
            Assert.AreEqual(new Match[] {
                    new Match("A", "A", 0)
            },
            matches, "Matches");
        }
    }
}
