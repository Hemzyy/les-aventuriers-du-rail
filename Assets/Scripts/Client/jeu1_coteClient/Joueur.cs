using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
public class Joueur
{
	// class qui represente le joueur

	// entier identifiant de maniere unique le joueur (a voir dans la base de donnees)
	public int Identifiant { get; private set; }

	// chaine de caractere qui represente le nom / pseudo du joueur
	public string Nom { get; private set; }


	// entier representant la couleur qu'on va associer au joueur
	public int Couleur { get; private set; }

	// les cartes missions du joueur (la main mission)
	public List<CarteMission> Missions { get; private set; }

	// les cartes wagons du joueur (la main wagon)
	public List<CarteWagon> Wagons { get; private set; }

	// le nombre de wagon restant chez un joueur
	public int WagonsRestants { get; private set; } = 45;



	// le constructeur prend en arguments le nom, l'identifiant, la couleur pour la version alpha
	// mais il doit toujours prendre un Gestionnaire de pioche, quelque soit la version
	public Joueur(int Identifiant, string Nom, int Couleur, List<CarteMission> missions, List<CarteWagon> wagons)
	{
		this.Identifiant = Identifiant;
		this.Nom = Nom;
		this.Couleur = Couleur;
		Missions = missions;
		Wagons = wagons;
	}


	// fonction qui ajoute une carte mission a la main du joueur
	public void ajouterMission(CarteMission cartePioche)
	{
		Missions.Add(cartePioche);
	}



	// fonction permettant d'ajouter une carte wagon a la main du joueur
	public void ajouterWagon(CarteWagon cartePioche)
	{
		Wagons.Add(cartePioche);
	}

	public void supprimerWagon(int couleur, int nombre)
	{
		for (int i = 0; i < Wagons.Count; i++)
		{
			if (Wagons[i].colorToInt() == couleur)
			{
				if (nombre == 0)
				{
					break;
				}
				Wagons.RemoveAt(i);
				nombre--;
			}
		}
	}



	// fonction responsable de la pose de wagons
	public void poserWagons(int nombre)
	{
		if (nombre > WagonsRestants)
		{
			throw new Exception("On ne peut pas poser plus de wagons que ceux qu'on possède !");
		}
		WagonsRestants -= nombre;
	}

}
