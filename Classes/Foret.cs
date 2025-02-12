using System.Linq;

namespace GenerationPlateau
{
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
			if(v1 == -1 || v2 == -1)
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
			for(int i = 0; i < Villes.Count; i++)
			{
				if(Villes[i] == ville1)
				{
					v1 = i;
				}
				if(Villes[i] == ville2)
				{
					v2 = i;
				}
			}
			CheckVilleNonTrouvee(v1, v2);
			
			Villes[v1].AddRoute(id, Villes[v2], distance);
		}

		private bool NotIn(Noeud elem, List<Noeud> liste)
		{
			foreach(Noeud e in liste)
			{
				if (elem == e)
				{
					return false;
				}
			}
			return true;
		}

		/*
		private List<int> DFSrec(Noeud n, List<int> routesVisitees, List<int> distances)
		{
			foreach (Voisin v in n.Routes)
			{
				if (NotIn(v.GetRoute(), routesVisitees))
				{
					routesVisitees.Add(v.GetRoute());
					distances[0] += v.GetDistance();
					if (distances[0] > distances[1])
					{
						distances[1] = distances[0];
					}
					distances = DFSrec(v.GetVille(), routesVisitees, distances);
					distances[0] -= v.GetDistance();
				}
			}
			return distances;
		}

		public int DFS(Noeud depart)
		{
			List<int> distances = [0, 0];
			List<int> routesVisitees = new();
			distances = DFSrec(depart, routesVisitees, distances);
			return distances[1];
		}

		public int CheminLePlusLong()
		{
			int distanceMax = 0;
			int inter;
			foreach(Noeud n in Villes)
			{
				inter = DFS(n);
				if(distanceMax < inter)
				{
					distanceMax = inter;
				}
			}
			return distanceMax;
		}
		*/

		private int CheminLePlusLongDepart(Noeud Depart, List<int> RoutesDejaVisitees, int DistanceActuelle)
		{
			int length = Depart.GetRoutes().Count;
			List<int> distancesCalculees = Enumerable.Repeat(0, length).ToList();
			//for(int i = 0; i<length; i++)
			Parallel.For(0, length, (i) =>
			{
				Voisin V = Depart.GetRoutes()[i];
				if(!RoutesDejaVisitees.Contains(V.GetRoute()))
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
				if(distance > distanceMax)
				{
					distanceMax = distance;
				}
			});
			return distanceMax;
		}
	}
}