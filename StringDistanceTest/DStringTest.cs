using NUnit.Framework;

namespace SSMKittel.StringDistance.Tests
{
    public class Tests
    {
        [TestCase("", "", 0)]
        [TestCase("A", "", 1)]
        [TestCase("A", "A", 0)]
        [TestCase("", "A", 1)]
        [TestCase("", "AB", 2)]
        [TestCase("AB", "AB", 0)]
        [TestCase("AB", "", 2)]
        [TestCase("", "ABC", 3)]
        [TestCase("ABC", "", 3)]
        [TestCase("ABC", "ABC", 0)]
        [TestCase("ABC", "BC", 1)]
        [TestCase("ABC", "CB", 2)]
        [TestCase("BC", "ABC", 1)]
        [TestCase("CB", "ABC", 2)]
        [TestCase("EPLYYYYYMDVWGK", "ELRPYYYYYMDVWGK", 2)]
        [TestCase("ELRPYYYYYMDVWGK", "EPLYYYYYMDVWGK", 2)]
        [TestCase("ADLTTGAR", "DPALTTGAR", 2)]
        [TestCase("DPALTTGAR", "ADLTTGAR", 2)]
        [TestCase("NDLWFR", "DGNLWFR", 2)]
        [TestCase("DGNLWFR", "NDLWFR", 2)]
        public void Edits(string first, string second, int dist)
        {
            uint? distance = (uint?)dist;
            Assert.AreEqual(distance, new DString(first, 0L).Edits(new DString(second, 0L)), "distance");
        }

        [TestCase("", "", 0)]
        [TestCase("A", "", 1)]
        [TestCase("A", "A", 0)]
        [TestCase("", "A", 1)]
        [TestCase("", "AB", 2)]
        [TestCase("AB", "AB", 0)]
        [TestCase("AB", "", 2)]
        [TestCase("", "ABC", 3)]
        [TestCase("ABC", "", 3)]
        [TestCase("ABC", "ABC", 0)]
        [TestCase("ABC", "BC", 1)]
        [TestCase("ABC", "CB", 2)]
        [TestCase("BC", "ABC", 1)]
        [TestCase("CB", "ABC", 2)]
        [TestCase("EPLYYYYYMDVWGK", "ELRPYYYYYMDVWGK", 2)]
        [TestCase("ELRPYYYYYMDVWGK", "EPLYYYYYMDVWGK", 2)]
        [TestCase("ADLTTGAR", "DPALTTGAR", 2)]
        [TestCase("DPALTTGAR", "ADLTTGAR", 2)]
        [TestCase("NDLWFR", "DGNLWFR", 2)]
        [TestCase("DGNLWFR", "NDLWFR", 2)]
        [TestCase("", "ABCD", null)]
        [TestCase("ABCD", "", null)]
        public void Distance(string first, string second, int? dist)
        {
            uint? distance = (uint?)dist;
            Assert.AreEqual(distance, new DString(first, 0L).Distance(new DString(second, 0L), 3), "distance");
        }
    }
}