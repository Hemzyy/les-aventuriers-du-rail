public class DefausseCartesMissions : EnsembleDeCartes
{
	// classe represantant la defausse des cartes mission (une structure de donnees utilise en interne)
	// elle implemente la classe abstraite ensemble de cartes

	// constructeur de la classe qui ne fait que appeler le constructeur parent
	public DefausseCartesMissions() : base() { }


	// fonction qui ne fait que defausser des cartes (ajouter les a la defausse)
	public void defausserCarte(CarteMission aDefausser)
	{
		ajouteCarte(aDefausser);
	}
}