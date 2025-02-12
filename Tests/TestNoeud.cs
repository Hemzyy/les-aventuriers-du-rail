namespace GenerationPlateau.Tests
{
    [TestFixture]
    public class NoeudTests
    {
        private Noeud _n0;
        private Noeud _n1;

        [SetUp]
        public void SetUp()
        {
            _n0 = new(0);
            _n1 = new(1);
        }

        [Test]
        public void TestAddRoute()
        {
            try
			{
				_n0.AddRoute(0, _n0, 1);
                Assert.Fail("Erreur d'ajout de route vers la ville elle-meme non detectee.");
			}
			catch (Exception ex)
			{
                Assert.IsTrue(ex is Exception);
			}

            _n0.AddRoute(0, _n1, 2);
			try
			{
				_n1.AddRoute(0, _n0, 4);
                Assert.Fail("Erreur d'ajout de routes non detectee.");
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex is Exception);
			}
        }
    }
}