public class PiocheCartesWagon : EnsembleDeCartes
{
	// classe represantant une pile de cartes wagon
	// elle implemente la classe abstraite ensemble de cartes
	

	// le constructeur ne fait que appeler une fonction privee qui initialize la pile
	public PiocheCartesWagon()
	{
		initializePioche();
	}
	

	// fonction qui initialize la pile des cartes wagon avec des cartes wagon aleatoire 
	// la pile a une taille initiale de 50 cartes (hard coded value) 
	// cette limite est du au fait qu'il peut exister plus d'une carte wagon avec le meme identifiant
	private void initializePioche()
	{
		CarteWagon carteAInserer;

		for (int i = 0; i < 50; i++) // je vais me permettre un taille fixe de la pioche / de cartes dans la pioche (de base)
		{
			carteAInserer = new CarteWagon();
			ajouteCarte(carteAInserer);
		}
		
		melangeCartes();
	}

	// fonction qui pioche une carte wagon
	// il faut juste verifier si la pile n'est pas vide avant
	public CarteWagon piocherCarte()
	{
		return (CarteWagon)retireCarte();
	}
}