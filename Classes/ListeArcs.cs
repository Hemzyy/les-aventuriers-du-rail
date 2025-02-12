namespace GenerationPlateau
{
	public class ListeArcs
	{
		private readonly List<Arc> Liste;
		
		public ListeArcs(List<Arc> liste)
		{
			this.Liste = liste;
            _ = Liste.OrderBy(l => l.GetId());
			CheckDoublons(VerifAucunDoublon());
		}


		//Fonctions vérifiant les erreurs => generation d'une exception

		private static void CheckDoublons(bool test) //verifie qu'aucun doublon n'existe
		{
			if (!test)
			{
				throw new Exception("Il ne peut pas y avoir deux fois la meme route !");
			}
		}
		

		//Fonctions get

		public List<Arc> GetListe()
		{
			return Liste;
		}
		
		private int IndiceArc(int id) //possibilite d'optimisation en faisant par dichotomie
		{
			for(int i = 0; i<Liste.Count; i++)
			{
				if(id == Liste[i].GetId())
				{
					return i;
				}
			}
			return -1;
		}

        private bool VerifAucunDoublon()
		{
			for(int i = 0; i<Liste.Count-1; i++)
			{
				if(Liste[i] == Liste[i+1])
				{
					return false;
				}
			}
			return true;
		}
		
		public bool Equals(ListeArcs other)
		{
			if(Liste.Count != other.GetListe().Count) return false;
			
			for(int i = 0; i<Liste.Count; i++)
			{
				if(Liste[i] != other.GetListe()[i])
				{
					return false;
				}
			}
			return true;
		}

		public Arc GetRoute(int idRoute)
		{
			int i = IndiceArc(idRoute);
			if(i == -1)
			{
				throw new Exception("La route n'existe pas");
			}

			return Liste[i];
		}

		public void AssignerJoueurARoute(int idJoueur, int idRoute)
		{
			int i = IndiceArc(idRoute);
			if(i == -1)
			{
				throw new Exception("La route n'existe pas");
			}

			Liste[i].SetPrise(idJoueur);
		}

		public override string ToString()
		{
			string res = "";
			foreach (Arc a in Liste)
			{
				res += $"{a}\n";
			}
			return res;
		}
	}
}