using System.Data;

public class CarteMission : Carte
{
	// classe qui implemente la classe abstraite Carte
	// cette classe represente une carte mission du cote du client

	// identifiant inherited de la class carte

	// entier qui indique la ville de depart de la carte mission en question (il est stocke comme un entier sur la base de donne)
	public string VilleDepart { get; private set; } = "-1";

	// entier qui indique la ville d'arrive de la carte mission en question (il est stocke comme un entier sur la base de donne)
	public string VilleArrive { get; private set; } = "-1";

	// entier qui indique le nombre de point qu'une carte mission va donner si accomplie, ou retire si non accomplie
	public int Points { get; private set; } = -1;


	public bool Accomplie { get; private set; } = false;




	// constructeur d'une carte mission qui ne produit pas de doublant
	public CarteMission(int identifiant, string villeDepart, string villeArrive, int points)
	{
		Identifiant = identifiant;
		VilleDepart = villeDepart;
		VilleArrive = villeArrive;
		Points = points;

	}


	public void finirCarteMission()
	{
		Accomplie = true;
	}



	// fonction permettant de transformer en string une carte mission
	public override string ToString()
	{
		return $"{base.ToString()}, VilleDepart={VilleDepart}, VilleArrive={VilleArrive}, Points={Points}";
	}
}
