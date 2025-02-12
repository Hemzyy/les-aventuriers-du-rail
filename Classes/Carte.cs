public abstract class Carte
{
	// il s'agit d'une classe abstraite represantant une carte
	// elle contient un attribut (ou plutot propriete) identifiant qui va identifier une carte donnes
	public int Identifiant { get; protected set; } = -1;

	// fonction permettant de transformer un objet Carte en string 
	public override string ToString()
	{
		return $"Carte: Identifiant={Identifiant}";
	}


	// fonction permettant de recuperer une instance d'une carte plus specifique (carte mission)
	public static CarteMission createCarteMission()
	{
		return new CarteMission();
	}

	// fonction permettant de recuperer une instance d'une carte plus specifique (carte wagon)
	public static CarteWagon createCarteWagon()
	{
		return new CarteWagon();
	}
}