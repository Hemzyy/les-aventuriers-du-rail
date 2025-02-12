namespace GenerationPlateau.Tests
{
    [TestFixture]
    public class ListeArcsTests
    {
        List<Arc> _listeDesArcs;
        ListeArcs _la;
        Arc _test;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _listeDesArcs =
            [
                new Arc(10, 1, 2, 0, 5, -1),
                new Arc(11, 2, 1, 1, 5, -1),
                new Arc(12, 1, 3, 2, 2, 0)
            ];
            _la = new(_listeDesArcs);
            _test = new(13, 1, 2, 0, 5, -1);
        }

        /*
        [SetUp]
        public void SetUp()
        {
            //Assert.IsTrue(_listeDesArcs is not null);
            _la = new(_listeDesArcs);
        }
        */

        [Test]
        public void TestInstanciationListeArcs()
        {
            List<Arc> testLA =
            [
                _test,
                _test
            ];

            try
            {
                ListeArcs testLa = new(testLA);
                Assert.Fail("Erreur de doublons non detectee.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exception);
            }
        }

        [Test]
        public void TestGetRoute()
        {
            //Assert.IsTrue(_la is not null);
            //Assert.IsTrue(_la.GetRoute(10) is Arc);

            try
            {
                _la.GetRoute(123);
                Assert.Fail("Erreur de route inexistante non detectee.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exception);
            }
        }

        [Test]
        public void TestAssignerJoueurARoute()
        {
            try
            {
                _la.AssignerJoueurARoute(1, 12);
                Assert.Fail("Erreur d'assignation d'un joueur a une route deja prise non detectee.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exception);
            }

            try
            {
                _la.AssignerJoueurARoute(2,123);
                Assert.Fail("Erreur d'assignation d'un joueur a une route inexistante non detectee.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exception);
            }
        }
    }
}