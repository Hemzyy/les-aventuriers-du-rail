using MySql.Data.MySqlClient;
using System.Linq;

public class Foret
{
	private readonly int IdJoueur;
	private List<Noeud> Villes;

	private static void CheckMemeVille(Noeud v1, Noeud v2) //verifie que les deux villes ne soient pas identiques
	{
		if (v1 == v2)
		{
			throw new Exception("On ne peut pas avoir la meme ville !");
		}
	}

	private static void CheckVilleNonTrouvee(int v1, int v2) //verifie que la ville existe quand elle le devrait
	{
		if (v1 == -1 || v2 == -1)
		{
			throw new Exception("Une des deux villes n'a pas ete trouvee !");
		}
	}

	public Foret(int idJoueur, List<Noeud> villes)
	{
		this.IdJoueur = idJoueur;
		this.Villes = villes;
	}

	public int GetIdJoueur()
	{
		return IdJoueur;
	}

	public List<Noeud> GetVilles()
	{
		return Villes;
	}

	public void AddArc(int id, Noeud ville1, Noeud ville2, int distance)
	{
		CheckMemeVille(ville1, ville2);
		int v1 = -1, v2 = -1;
		for (int i = 0; i < Villes.Count; i++)
		{
			if (Villes[i] == ville1)
			{
				v1 = i;
			}
			if (Villes[i] == ville2)
			{
				v2 = i;
			}
		}
		CheckVilleNonTrouvee(v1, v2);

		Villes[v1].AddRoute(id, Villes[v2], distance);
	}

	private bool NotIn(Noeud elem, List<Noeud> liste)
	{
		foreach (Noeud e in liste)
		{
			if (elem == e)
			{
				return false;
			}
		}
		return true;
	}

	private int CheminLePlusLongDepart(Noeud Depart, List<int> RoutesDejaVisitees, int DistanceActuelle)
	{
		int length = Depart.GetRoutes().Count;
		List<int> distancesCalculees = Enumerable.Repeat(0, length).ToList();
		//for(int i = 0; i<length; i++)
		Parallel.For(0, length, (i) =>
		{
			Voisin V = Depart.GetRoutes()[i];
			if (!RoutesDejaVisitees.Contains(V.GetRoute()))
			{
				List<int> RDV = new List<int>(RoutesDejaVisitees);
				RDV.Add(V.GetRoute());
				distancesCalculees[i] = CheminLePlusLongDepart(V.GetVille(), RDV, DistanceActuelle + V.GetDistance());
			}
		});
		int res = distancesCalculees.Max();
		return res > 0 ? res : DistanceActuelle;
	}

	public int CheminLePlusLong()
	{
		int distanceMax = 0;
		int distance = 0;
		//foreach(Noeud N in Villes)
		Parallel.ForEach(Villes, (N) =>
		{
			distance = CheminLePlusLongDepart(N, new List<int>(), 0);
			if (distance > distanceMax)
			{
				distanceMax = distance;
			}
		});
		return distanceMax;
	}

	public Noeud RecupererNoeud(int valeurNoeud)
	{
		for (int i = 0; i < Villes.Count; i++)
		{
			if (Villes[i].GetVille() == valeurNoeud)
			{
				return Villes[i];
			}
		}
		throw new Exception("Le noeud n'existe pas !");
	}

	private int ModifieNoeud(List<(Noeud, int)> liste, Noeud n, int d)
	{
		for (int i = 0; i < liste.Count; i++)
		{
			if (liste[i].Item1 == n)
			{
				int inter = liste[i].Item2;
				if (inter > d || inter == -1)
				{
					liste[i] = (liste[i].Item1, d);
				}
				return liste[i].Item2;
			}
		}
		throw new Exception("Le noeud n'a pas ete trouve !");
	}

	private Noeud PlusProche(List<(Noeud, int)> distances, List<Noeud> visites, Noeud noeud)
	{
		int minimum = -1;
		int inter;
		Noeud nMin = new(-1);
		foreach (Voisin v in noeud.GetRoutes())
		{
			Noeud voisinVille = v.GetVille();
			if (NotIn(voisinVille, visites))
			{
				inter = ModifieNoeud(distances, voisinVille, v.GetDistance());
				if (inter < minimum || minimum == -1)
				{
					minimum = inter;
					nMin = voisinVille;
				}
			}
		}

		if (minimum == -1)
		{
			throw new Exception("Il n'y a pas de noeud plus proche !");
		}
		return nMin;
	}

	private bool VoisinsNonVisites(List<Noeud> visites, Noeud n)
	{
		foreach (Voisin v in n.GetRoutes())
		{
			if (NotIn(v.GetVille(), visites))
			{
				return true;
			}
		}
		return false;
	}

	public bool Dijkstra(Noeud depart, Noeud arrivee)
	{
		if (depart == arrivee)
		{
			return true;
		}

		List<(Noeud, int)> distances = new();
		List<Noeud> visites = new();
		Noeud noeudCourant = depart;
		visites.Add(depart);
		distances.Add((depart, 0));

		for (int i = 0; i < Villes.Count; i++)
		{
			distances.Add((Villes[i], -1));
		}

		ModifieNoeud(distances, depart, 0);

		do
		{
			noeudCourant = PlusProche(distances, visites, noeudCourant);
			visites.Add(noeudCourant);
			if (noeudCourant == arrivee)
			{
				return true;
			}
		}
		while (VoisinsNonVisites(visites, noeudCourant));

		return false;
	}

	public static List<Noeud> getVilles()
	{
		List<Noeud> villes = new List<Noeud>();
        string connectionString = "Server=192.168.100.102;Database=LesAventuriersDuRail;UserId=ghait;Password=mdpDeOuf_2024;";

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			connection.Open();

			MySqlCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Ville";

			using (MySqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					villes.Add(new Noeud(reader.GetInt32("idVille")));
				}
			}
		}

		return villes;
	}



}