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

        [TestCase(0U, 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000UL)]
        [TestCase(8U, 0b11111111_00000000_00000000_00000000_00000000_00000000_00000000_00000000UL)]
        [TestCase(8U, 0b00000000_11111111_00000000_00000000_00000000_00000000_00000000_00000000UL)]
        [TestCase(8U, 0b00000000_00000000_11111111_00000000_00000000_00000000_00000000_00000000UL)]
        [TestCase(8U, 0b00000000_00000000_00000000_11111111_00000000_00000000_00000000_00000000UL)]
        [TestCase(8U, 0b00000000_00000000_00000000_00000000_11111111_00000000_00000000_00000000UL)]
        [TestCase(8U, 0b00000000_00000000_00000000_00000000_00000000_11111111_00000000_00000000UL)]
        [TestCase(8U, 0b00000000_00000000_00000000_00000000_00000000_00000000_11111111_00000000UL)]
        [TestCase(8U, 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_11111111UL)]
        [TestCase(32U, 0b00000000_11111111_00000000_11111111_00000000_11111111_00000000_11111111UL)]
        [TestCase(32U, 0b11111111_00000000_11111111_00000000_11111111_00000000_11111111_00000000UL)]
        [TestCase(64U, 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_11111111UL)]
        public void PopCount(uint popcount, ulong value)
        {
            Assert.AreEqual(popcount, DString.popcount64b(value), "popcount");
        }
    }
}
