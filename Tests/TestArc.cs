namespace GenerationPlateau.Tests
{
    [TestFixture]
    public class ArcTests
    {
        private Arc _a;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _a = new(0, 0, 1, 0, 1, 2);
        }

        [Test]
        public void TestInstanciationArc()
        {
            try
            {
                Arc a0 = new(0, 0, 2, 0, -1, 0);
                Assert.Fail("Erreur d'id non unique non detectee.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exception);
            }

            try
            {
                Arc a1 = new(1, 0, 2, 0, -1, 0);
                Assert.Fail("Erreur de longueur non detectee.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exception);
            }

            try
			{
				Arc a2 = new(2, 0, 0, 0, 2, 1);
                Assert.Fail("Erreur de villes non detectee.");
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is Exception);
			}

            try
			{
				Arc a3 = new(3, 0, 0, 0, 2, -2);
                Assert.Fail("Erreur du parametre prise bien detectee.");
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is Exception);
			}
        }

        [Test]
        public void TestSetPrise()
        {
            try
			{
				_a.SetPrise(-3);
                Assert.Fail("Erreur de setPrise (parametre prise invalide) non detectee.");
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is Exception);
			}

            try
			{
				_a.SetPrise(2);
                Assert.Fail("Erreur de setPrise (route deja affectee) non detectee.");
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is Exception);
			}
        }
    }
}