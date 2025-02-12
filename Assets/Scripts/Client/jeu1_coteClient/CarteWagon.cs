using System.Data;
public class CarteWagon : Carte
{
	// classe qui implemente la classe abstraite carte
	// elle represente une carte wagon

	// entier representant une couleur (comme stocke sur la base)
	public string Couleur { get; private set; }



	// le constructeur qui permet de produire une carte wagon apres reception de la carte a travers le reseau
	public CarteWagon(int identifiant, string couleur)
	{
		Identifiant = identifiant;
		Couleur = couleur;
	}


	// fonction qui retourne la couleur sous forme de int
	public int colorToInt()
	{
		return Identifiant;
	}


	// fonction permettant de transformer en string une carte wagon
	public override string ToString()
	{
		return $"{base.ToString()}, Couleur={Couleur}";
	}
}
