using GenerationPlateau;
using System;
using System.Collections.Generic;

public class JeuServeur
{
    public List<Joueur> Joueurs { get; private set; }
    public int IdentifiantJoueurActuel { get; private set; }
    public bool JeuTermine { get; private set; }

    // initialiser la carte dans une ListeArcs
    public ListeArcs Map { get; private set; }

    // permet d'initialiser les graphes pour les joueurs
    public List<Noeud> Villes { get; private set; }

    // permet de stocker les graphes des joueurs
    public List<Foret> Graphes { get; private set; }

    public GestionnairePioche pileManager;


    public JeuServeur(ListeArcs map, List<Noeud> villes)
    {
        Joueurs = new List<Joueur>();

        IdentifiantJoueurActuel = 0;
        JeuTermine = false;
        Map = map;
		Villes = villes;
		Graphes = new List<Foret>();
        pileManager = new GestionnairePioche();
    }

    public void AjouterJoueur(Joueur joueur)
    {
        Joueurs.Add(joueur);
        Graphes.Add(new Foret(joueur.Identifiant, Villes));
    }

    public Joueur GetJoueur(int id)
    {
        return Joueurs.Find(joueur => joueur.Identifiant == id);
    }

    public void VerifierFinDeJeu(Joueur joueur)
    {
        // Exemple de vérification basique
        if (joueur.WagonsRestants <= 2)
        {
            JeuTermine = true;
        }
    }

    public void PasserAuJoueurSuivant()
    {
        // changer pour pouvoir utiliser la liste des Joueurs a notre disposition
        IdentifiantJoueurActuel = (IdentifiantJoueurActuel + 1) % Joueurs.Count;
        Console.WriteLine("IdentifiantJoueurActuel    " +IdentifiantJoueurActuel);
    }

    private void AjouterScoreJoueur(List<(int, int)> scores, int identifiant)
    {
        for (int i = 0; i < scores.Count; i++)
        {
            if (scores[i].Item1 == identifiant)
            {
                scores[i] = (scores[i].Item1, scores[i].Item2 + 10);
            }
        }
    }

    private List<int> MaximumScore(List<(int, int)> scores)
    {
        List<int> idMax = new();
        idMax.Add(scores[0].Item1);
        int valMax = scores[0].Item2;
        int valInter;

        for (int i = 1; i < scores.Count; i++)
        {
            valInter = scores[i].Item2;
            if (valInter > valMax)
            {
                valMax = valInter;
                idMax.Clear();
                idMax.Add(scores[i].Item1);
            }
            else if (valInter == valMax)
            {
                idMax.Add(scores[i].Item1);
            }
        }

        return idMax;
    }

    public List<int> DeterminerGagnant() // renvoie la liste du / des premiers
    {
        // Algorithme pour déterminer le gagnant
        List<(int, int)> scores = new();
        int scoreIntermediaire;

        int longueurPlusLongChemin = -1;
        int joueurPlusLongChemin = -1;
        int longueurIntermediaire;

        int identifiant;

        foreach (Joueur j in Joueurs)
        {
            scoreIntermediaire = j.Score;
            identifiant = j.Identifiant;
            foreach (CarteMission cm in j.Missions)
            {
                if (cm.Accomplie)
                {
                    scoreIntermediaire += cm.Points;
                }
                else
                {
                    scoreIntermediaire -= cm.Points;
                }
            }
            Foret f = GetForetJoueur(j);
            longueurIntermediaire = f.CheminLePlusLong();
            if (longueurPlusLongChemin < longueurIntermediaire)
            {
                longueurPlusLongChemin = longueurIntermediaire;
                joueurPlusLongChemin = identifiant;
            }

            scores.Add((identifiant, scoreIntermediaire));
        }

        AjouterScoreJoueur(scores, joueurPlusLongChemin);
        //Console.WriteLine("Le jeu est terminé. Le gagnant est ...");
        return MaximumScore(scores);
    }

    public CarteWagon DemandePileWagon(Joueur joueur)
    {
        CarteWagon card = joueur.piocherPileWagon();
        joueur.ajouterWagon(card);
        return card;
    }

    public CarteWagon DemandeTableWagon(Joueur joueur, int idCarte)
    {
        CarteWagon card = joueur.piocherTableWagon(idCarte);
        joueur.ajouterWagon(card);
        return card;
    }

    public CarteMission[] DemandeMissions(Joueur joueur)
    {
        CarteMission[] cards = joueur.piocherMission();
        foreach (var card in cards)
        {
            joueur.ajouterMission(card);
        }
        return cards;
    }

	public (List<CarteMission>, List<CarteMission>) EnsembleChoix(CarteMission[] possibilites, bool[] choix) //choix a la meme longueur que possibilites
	{
		List<CarteMission> missionsChoisies = new List<CarteMission>();
		List<CarteMission> missionsAJeter = new List<CarteMission>();
		for (int i = 0; i < choix.Length; i++)
		{
			if (choix[i])
			{
				missionsChoisies.Add(possibilites[i]);
			}
			else
			{
				missionsAJeter.Add(possibilites[i]);
			}
		}

		return (missionsChoisies, missionsAJeter);
	}

    public void ChoixMissions(Joueur joueur, List<CarteMission> choix)
    {
        foreach (CarteMission cm in choix)
        {
            joueur.ajouterMission(cm);
        }
    }
    // penser a jeter les cartes missions en trop

    public bool CheminExiste(Foret f, int depart, int arrivee)
    {
        Noeud nDepart = f.RecupererNoeud(depart);
        Noeud nArrivee = f.RecupererNoeud(arrivee);
        return f.Dijkstra(nDepart, nArrivee);
    }

    public void ParcourirMissions(Joueur joueur)
    {
        foreach (CarteMission cm in joueur.Missions)
        {
            if (!(cm.Accomplie))
            {
                Foret f = GetForetJoueur(joueur);
                // CheminExiste renvoie true si un chemin existe
                cm.Accomplie = CheminExiste(f, cm.VilleDepart, cm.VilleArrive);
            }
        }
    }

    private Foret GetForetJoueur(Joueur joueur)
    {
        foreach (Foret f in Graphes)
        {
            if (f.GetIdJoueur() == joueur.Identifiant)
            {
                return f;
            }
        }

        throw new Exception("Ce joueur n'a pas de graphe !");
    }

    public bool DemandeAjoutDeRoute(Joueur joueur, int idRoute) // return false si une erreur s'est produite (surement due a idRoute)
    {
        try
        {
            AjoutDeRoute adr = new(Map, joueur, idRoute);

            // Ajouter la route dans le graphe du joueur
            Foret f = GetForetJoueur(joueur);
            (int, int, int, int) villes = Map.GetVilles(idRoute);
            Noeud ville1 = f.RecupererNoeud(villes.Item2);
            Noeud ville2 = f.RecupererNoeud(villes.Item3);
            f.AddArc(villes.Item1, ville1, ville2, villes.Item4);

            ParcourirMissions(joueur);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
    
    

}