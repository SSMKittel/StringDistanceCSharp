using NUnit.Framework;

namespace SSMKittel.StringDistance.Tests
{
    public class MatcherBuilderTest
    {
        [Test]
        public void RandomStrings()
        {
            FuzzyMatcher f = MatcherBuilder.Matcher(new string[] {
                    "HAAVGCA",
                    "VQQOLBT",
                    "VGOPXJT",
                    "ZKDEGLB",
                    "KUFQKUC",
                    "OPDXAAF",
                    "YTSAHCY",
                    "SQTSQKC",
                    "YDCPBTY",
                    "UAVJKPY",
                    "YPLZTWY",
                    "EJGXVWR",
                    "RRDTSVG",
                    "JVEKZZL",
                    "KAZQYDL",
                    "NJEPOWE",
                    "HRQJZTW",
                    "LQWINPT",
                    "WDZWDAW",
                    "PEZHQGI",
                    "TVQGZVVOZVUCTDK",
                    "TXXDOJDQHJGXCAC",
                    "FZSDAIFPSSIGHSM",
                    "JTIYUHFSLKIRDBD",
                    "WYFMXYBAJDVPKOI",
                    "POLPNKOGMEJZTEU",
                    "HDLBEEKYKRSUVQY",
                    "OOQECMVHUGEXISF",
                    "MJOZGUPVKOHCSRP",
                    "DPQRCNYRRBOFHGZ"
            });
            Assert.AreEqual("AAABCCDDEEFFGHIIJJKKLMNOOPPQQRRRSSSSTTUUVVVVWWWXXXYYZZ", f.Signature);
        }

        [Test]
        public void AlphabeticallySorted()
        {
            FuzzyMatcher f = MatcherBuilder.Matcher(new string[] {
                    "AAACGHV",
                    "AADFOPX",
                    "ABDFIJKMOPVWXYY",
                    "ACCDDGHJJOQTXXX",
                    "ACHSTYY",
                    "ADDWWWZ",
                    "ADFFGHIIMPSSSSZ",
                    "ADKLQYZ",
                    "AJKPUVY",
                    "BCDFGHNOPQRRRYZ",
                    "BCDPTYY",
                    "BDDFHIIJKLRSTUY",
                    "BDEEHKKLQRSUVYY",
                    "BDEGKLZ",
                    "BLOQQTV",
                    "CDGKOQTTUVVVVZZ",
                    "CEEFGHIMOOQSUVX",
                    "CFKKQUU",
                    "CGHJKMOOPPRSUVZ",
                    "CKQQSST",
                    "DGRRSTV",
                    "EEGJKLMNOOPPTUZ",
                    "EEJNOPW",
                    "EGHIPQZ",
                    "EGJRVWX",
                    "EJKLVZZ",
                    "GJOPTVX",
                    "HJQRTWZ",
                    "ILNPQTW",
                    "LPTWYYZ"
            });
            Assert.AreEqual("AAABCCDDEEFFGHIIJJKKLMNOOPPQQRRRSSSSTTUUVVVVWWWXXXYYZZ", f.Signature);
        }

        [Test]
        public void NoInput()
        {
            Assert.AreEqual(new MatcherBuilder().Build().Signature, "");
        }

        [Test]
        public void Pruned()
        {
            FuzzyMatcher f = MatcherBuilder.Matcher(new string[] {
                    "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-_abcdefghijklmnopqrstuvwxyz",
                    "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-_abcdefghijklmnopqrstuvwxyz",
                    "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-_abcdefghijklmnopqrstuvwxyz",
                    "+",
                    "\\",
                    "/"
            });
            Assert.AreEqual("-0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz", f.Signature);
        }
    }
}
