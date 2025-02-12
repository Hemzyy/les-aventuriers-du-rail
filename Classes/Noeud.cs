namespace GenerationPlateau
{
	public class Noeud
	{
		private readonly int Ville;
		private List<Voisin> Routes;
		
		public Noeud(int ville)
		{
			this.Ville = ville;
			Routes = new List<Voisin>();
		}
		

		//Fonctions get

		public int GetVille()
		{
			return Ville;
		}
		
		public List<Voisin> GetRoutes()
		{
			return Routes;
		}
		
		private void AddToList(int id, Noeud other, int distance)
		{
			Voisin v = new(id, other, distance);
			Routes.Add(v);
		}
		
		private int SearchInList(Noeud other)
		{
			int test = -1;
			for(int i = 0; i < Routes.Count; i++)
			{
				if(Routes[i].GetVille() == other)
				{
					test = i;
				}
			}
			return test;
		}
		
		public void AddRoute(int id, Noeud other, int distance)
		{
			if(this == other)
			{
				throw new Exception("On ne peut pas relier une ville a elle-meme directement !");
			}
			int i = SearchInList(other);
			if (i != -1)
			{
				throw new Exception("On ne peut pas ajouter une ville deja ajoutee !");
			}
			
			this.AddToList(id, other, distance);
			other.AddToList(id, this, distance);
		}
		
		public bool Equals(Noeud other)
		{
			return Ville == other.GetVille();
		}
	}
}