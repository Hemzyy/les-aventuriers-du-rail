
public class GestionnairePioche
{
	// classe qui va gerer toutes les pioches du jeu 
	// elle va potentiellement permettre le remplissage des la pile mission a partir de la defausse 
	// et elle va gerer toutes les possibilite de tirage possible, toutes pile / defausse comprise

	// une pioche mission
	public PiocheCartesMission PileMission { get; private set; }
	// une defausse mission
	public DefausseCartesMissions DiscardMission { get; private set; }
	// une pioche wagon
	public PiocheCartesWagon PileWagon { get; private set; }
	// un table de cartes wagon representant les cartes wagons disponibles sur tables (on peut pioche de la pile ou des cartes sur table)
	public CarteWagon[] SurTable { get; private set; }


	// constructeur qui va initialiser toutes les piles et le tableau surTable
	public GestionnairePioche()
	{
		PileMission = new PiocheCartesMission();
		DiscardMission = new DefausseCartesMissions();
		PileWagon = new PiocheCartesWagon();


		// on va creer un tableau avec moins de trois cartes identique
		// verification de cette condition se fait par la fonction verifySurTable
		do
		{
			SurTable = initializeSurTable();
		} while (verifySurTable());

	}
	// je ne considere pas d'autres constructeur parce que une gestionnaire doit etre creer et on ne peut pas lui passer de pioche particuliere

	// fonction qui initialize le tableau surTable a partir de la pioche wagon qu'on a
	private CarteWagon[] initializeSurTable()
	{
		CarteWagon[] surTable = new CarteWagon[5];
		for (int i = 0; i < surTable.Length; i++)
		{
			if (PileWagon.estVide() == true)
			{
				replenishPiocheWagon();
			}
			surTable[i] = PileWagon.piocherCarte();
		}

		return surTable;
	}
	


	// fonction qui permet de piocher a partir des cartes dispo sur table 
	// il lui faut juste l'indice de la carte qu'on veut piocher
	// elle se charge aussi de remettre une nouvelle carte a la place de celle qu'on va donner a l'utilisateur
	// elle ne verifie pas si le tirage etait un tirage d'une simple carte wagon ou d'une locomotive (a verifier par la fonction appelante)
	public CarteWagon piocherSurTable(int indiceCarte)
	{
		CarteWagon aRetourner = SurTable[indiceCarte];
		do
		{

			replenishSurTable(indiceCarte);
		} while(verifySurTable());
		return aRetourner;
	}

	// fonction permettant de fill sur table apres tirage d'une de ses cartes
	// elle est automatiquement appele apres tirage depuis la table
	private void replenishSurTable(int indiceCarte)
	{
		if (PileWagon.estVide() == true)
		{
			replenishPiocheWagon();
		}
		SurTable[indiceCarte] = (CarteWagon)PileWagon.retireCarte();
	}

	// fonction qui verifie si ce qui est sur table est correct
	// elle verifie si ce qui est sur table ne contient pas trois cartes de la meme couleur
	// je dois verifier si une autre condition s'applique au cartes multicouleur (locomotives)
	private bool verifySurTable()
	{
		bool tableInvalide = false;

		for (int i = 0; i < SurTable.Length; i++)
		{
			int count = 1; // Start counting from 1 for the current element
			for (int j = 0; j < SurTable.Length; j++)
			{
				if (i != j && SurTable[i].Identifiant == SurTable[j].Identifiant)
				{
					count++;
				}
			}
			if (count >= 3)
			{
				tableInvalide = true;
				break;
			}
		}

		return tableInvalide;
	}

	// fonction qui reremplit la pioche des cartes wagon (elle en recree une)
	public void replenishPiocheWagon()
	{
		PileWagon = new PiocheCartesWagon();
	}

	// fonction qui pioche une simple carte wagon 
	public CarteWagon piocherCartesWagon()
	{
		CarteWagon aRetourner;
		if (PileWagon.estVide() == true)
		{
			replenishPiocheWagon();
		}
		aRetourner = PileWagon.piocherCarte();
		return aRetourner;
	}


	// fonction replenish qui reremplit la pile des cartes mission a partir de la defausse des cartes mission
	public void replenishPiocheMission()
	{
		while (DiscardMission.estVide() != false)
		{
			PileMission.ajouteCarte(DiscardMission.retireCarte());
		}
		PileMission.melangeCartes();
	}

	// une fonction qui permet de pioches des cartes mission depuis la pioche des cartes mission 
	// on ne peut bien sur piocher que 3 cartes mission par tirage (on ne peut pas pioche plus ou moins)
	// il faut potentiellement voir pour les access aux fonctions de piocheCarteMission (individuel)
	// ou changer l'access de la propriete pile mission
	public CarteMission[] piocherCarteMission()
	{
		CarteMission[] aRetourner = new CarteMission[3];
		for (int i = 0; i < aRetourner.Length; i++)
		{
			if (PileMission.estVide() == true)
			{
				replenishPiocheMission();
			}
			aRetourner[i] = PileMission.piocheCarte();
		}
		return aRetourner;
	}

	public void defausserMission(CarteMission aDefausser)
	{
		DiscardMission.defausserCarte(aDefausser);
	}
}
