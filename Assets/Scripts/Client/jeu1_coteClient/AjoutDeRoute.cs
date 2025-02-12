using System.ComponentModel.Design;
using System.Collections.Generic;
using System;
public class AjoutDeRoute
{
	private ListeArcs Carte;
	private Joueur Player;
	private int Route;

	public AjoutDeRoute(ListeArcs carte, Joueur player, int route)
	{
		this.Carte = carte;
		this.Player = player;
		this.Route = route;
		AjouterRoute();
	}

	public static void CheckNombre(int nombre, int comparaison)
	{
		//Assert.IsTrue(nombre == comparaison);
		if (nombre != comparaison)
		{
			throw new Exception("Les cartes necessaires n'ont pas toutes ete supprimees !");
		}
	}

	public int DemanderCouleur()
	{
		Console.WriteLine("Veuillez choisir une couleur :");
		//string input = Console.ReadLine();
		//return int.Parse(input);
		return 1;
	}

	public bool SupprimerWagon(int nombre)
	{
		// if (Player.WagonsRestants < nombre)
		// {
		// 	throw new Exception("Il n'y a pas assez de wagons pour prendre cette route !");
		// }
		Player.poserWagons(nombre);

		if (Player.WagonsRestants <= 2)
		{
			// pour indiquer la fin du jeu
			return true;
		}
		return false;
	}

	// public int SupprimerCarteWagon(int couleur, int nombre)
	// {
	// 	List<CarteWagon> cartesEnMain = Player.Wagons;
	// 	for (int i = cartesEnMain.Count - 1; i >= 0; i--)
	// 	{
	// 		if (nombre != 0 && cartesEnMain[i].Couleur == couleur)
	// 		{
	// 			cartesEnMain.RemoveAt(i);
	// 			nombre--;
	// 		}
	// 	}
	// 	return nombre;
	// }

	public List<int> VerifieCartesCouleur(int couleur, int nombre)
	{
		List<CarteWagon> cartesEnMain = Player.Wagons;
		List<int> nbCartes = new List<int> { 0, 0 };
		for (int i = 0; i < cartesEnMain.Count; i++)
		{
			if (cartesEnMain[i].colorToInt() == 0)
			{
				nbCartes[1]++;
			}
			else if (cartesEnMain[i].colorToInt() == couleur)
			{
				nbCartes[0]++;
			}
		}
		return nbCartes;
	}

	public bool AjouterRoute()
	{
		Carte.AssignerJoueurARoute(Player.Identifiant, Route);
		Arc a = Carte.GetRoute(Route);
		int couleur = a.Couleur;
		int longueur = a.Longueur;

		bool finDePartie = SupprimerWagon(longueur);

		if (couleur == 0)
		{
			couleur = DemanderCouleur();
		}
		List<int> res = VerifieCartesCouleur(couleur, longueur);
		if (res[0] >= longueur)
		{
			// suppression = SupprimerCarteWagon(couleur, longueur);
			// CheckNombre(suppression, 0);
			Player.supprimerWagon(couleur, longueur);
		}
		else if (res[0] + res[1] >= longueur)
		{
			// suppression = SupprimerCarteWagon(couleur, res[0]);
			// CheckNombre(suppression, longueur - res[0]);
			Player.supprimerWagon(couleur, res[0]);

			// suppression = SupprimerCarteWagon(0, longueur - res[0]);
			// CheckNombre(suppression, 0);
			Player.supprimerWagon(0, longueur - res[0]);
		}
		else
		{
			throw new Exception("Il n'y a pas assez de cartes pour prendre la route !");
		}

		return finDePartie;
	}
}
