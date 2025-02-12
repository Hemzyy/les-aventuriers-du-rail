public class PiocheCartesMission : EnsembleDeCartes
{

	// classe qui implemente la classe Ensemble de Cartes

	// constructeur faisant appel a une fonction prive qui se charge d'initializer une pile de carte mission
	public PiocheCartesMission() : base()
	{
		initializePioche();
	}


	// fonction qui remplie la pile de cartes mission avec toutes les cartes disponible de la base de donnes
	private void initializePioche()
	{	
		CarteMission.resetVisited();
		CarteMission carteAInserer;

		do
		{

			carteAInserer = new CarteMission();
			if (carteAInserer.Identifiant == -1)
			{
				break;
			}
			ajouteCarte(carteAInserer);

		} while (true);

		melangeCartes();
	}


	// fonction qui permet de piocher une carte de la pile et la retourne
	// il faut verifier si la pile n'est pas vide avant de retirer une carte de la pile
	public CarteMission piocheCarte()
	{
		return (CarteMission)retireCarte();
	}
}