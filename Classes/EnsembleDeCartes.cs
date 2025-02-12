public abstract class EnsembleDeCartes
{

	// classe abstraite qui va representer une pile quelconque de cartes (une pile ou une defausse) 
	
	// la pile cartes (de n'importe quel type : cartes missions ou wagon)
	public Stack<Carte> Cartes { get; protected set; }


	// constructeur qui ne fait que creer la pile cartes
	public EnsembleDeCartes()
	{
		Cartes = new Stack<Carte>();
	}


	// fonction qui ajoute une element a la pile
	public void ajouteCarte(Carte carte)
	{
		Cartes.Push(carte);
	}


	// fonction qui retire un element a la pile
	public Carte retireCarte()
	{
		return Cartes.Pop();
	}


	// fonction qui fait melanger les cartes de la pile
	public void melangeCartes()
	{
		var cartesTab = Cartes.ToArray();
		Cartes.Clear();
		Random randomGenerator = new Random();
		int tailleCartesTab = cartesTab.Length;

		while (tailleCartesTab > 1)
		{
			int i = randomGenerator.Next(tailleCartesTab);
			tailleCartesTab--;

			Carte swapVar = cartesTab[i];
			cartesTab[i] = cartesTab[tailleCartesTab];
			cartesTab[tailleCartesTab] = swapVar;
		}

		foreach (var card in cartesTab)
		{
			ajouteCarte(card);
		}
	}


	// fonction qui verifie si la pile est vide ou pas
	public bool estVide()
	{
		return Cartes.Count == 0;
	}

	// fonction permettant de recuperer une instance d'une pile / defausse specefique
	public static PiocheCartesMission createPiocheMission()
	{
		return new PiocheCartesMission();
	}

	public static PiocheCartesWagon createPiocheWagon()
	{
		return new PiocheCartesWagon();
	}

	public static DefausseCartesMissions createDefausseMisison()
	{
		return new DefausseCartesMissions();
	}



}