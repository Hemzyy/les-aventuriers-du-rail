using System.Linq;

namespace GenerationPlateau.Tests
{
    [TestFixture]
    public class ForetTests
    {
        private Foret _f;

        [SetUp]
        public void SetUp()
        {
            List<Noeud> f1 =
            [
                new Noeud(1),
                new Noeud(2),
                new Noeud(3),
                new Noeud(4),
                new Noeud(5)
            ];
            _f = new(1, f1);
        }

        [Test]
        public void TestAddArc()
        {
            List<Noeud> ln = _f.GetVilles();
            try
            {
                _f.AddArc(0, ln[0], ln[0], 1);
                Assert.Fail("Erreur de meme ville non detectee.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exception);
            }

            Noeud test = new(44);
            try
            {
                _f.AddArc(1, ln[0], test, 2);
                Assert.Fail("Erreur de ville inexistante non detectee.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exception);
            }

            _f.AddArc(2, ln[0], ln[1], 4);
            Assert.IsTrue(ln[0].GetRoutes().Any(v => v.GetVille() == ln[1]), "Le Noeud ln[1] n'est pas trouvé dans la liste des routes de ln[0].");
        }

        [Test]
        public void TestCheminLePlusLong()
        {
            List<Noeud> ln = _f.GetVilles();
            _f.AddArc(0, ln[0], ln[1], 1);
            _f.AddArc(1, ln[0], ln[2], 1);
            _f.AddArc(2, ln[0], ln[3], 1);
            _f.AddArc(3, ln[1], ln[2], 1);
            _f.AddArc(4, ln[1], ln[3], 1);
            _f.AddArc(5, ln[2], ln[3], 1);
            _f.AddArc(6, ln[2], ln[4], 1);
            _f.AddArc(7, ln[3], ln[4], 1);

            int res = _f.CheminLePlusLong();
            Assert.IsTrue(res == 8);
        }
    }
}