using System;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

class Jeu
{
	public List<Joueur> Joueurs { get; private set; }
	public int IdentifiantJoueurActuel { get; private set; }
	public TimerTour Chrono { get; private set; }
	public bool JeuTermine { get; private set; }
	public GestionnairePioche PileManager { get; private set; }

	public Jeu()
	{
		Joueurs = new List<Joueur>();

		IdentifiantJoueurActuel = 0;
		JeuTermine = false;

		PileManager = new GestionnairePioche();

		Chrono = new TimerTour(60); // 60 secondes pour chaque tour
	}

	public void ajouterJoueur(Joueur joueur)
	{
		Joueurs.Add(joueur);
	}


	public void Commencer()
	{
		for (int i = 0; i < 10; i++)
		//while (!JeuTermine)
		{
			// a verifier 
			Joueur joueurActuel = Joueurs[IdentifiantJoueurActuel];
			Console.WriteLine($"C'est le tour de {joueurActuel.Nom}.");


			// Simuler le choix d'une action par le joueur (à remplacer par l'algo final)
			SimulerActionsJoueur(joueurActuel);


			// Vérification de la condition de fin de jeu (à implémenter)
			VerifierFinDeJeu(joueurActuel);

			PasserAuJoueurSuivant();
		}

		DeterminerGagnant();
	}

	private void SimulerActionsJoueur(Joueur joueur)
	{
		Chrono.Demarrer();

		// Exemple d'actions
		//joueur.PiocherCarteMission(piocheMissions);
		// Ajouter d'autres actions ici de la classe Pioche carte missions

		Chrono.Arreter();
	}

	public void AjouterJoueur(Joueur joueur)
	{
		Joueurs.Add(joueur);
	}

	private void VerifierFinDeJeu(Joueur joueur)
	{
		// Exemple de vérification basique
		if (joueur.WagonsRestants <= 2)
		{
			JeuTermine = true;
		}
	}

	private void PasserAuJoueurSuivant()
	{
		// changer pour pouvoir utiliser la liste des Joueurs a notre disposition
		IdentifiantJoueurActuel = (IdentifiantJoueurActuel + 1) % Joueurs.Count;
	}

	private void DeterminerGagnant()
	{
		// Algorithme pour déterminer le gagnant
		Console.WriteLine("Le jeu est terminé. Le gagnant est ...");
	}

}