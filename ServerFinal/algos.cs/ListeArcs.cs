
using MySql.Data.MySqlClient;

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

	public (int, int, int, int) GetVilles(int idRoute)
	{
		return GetRoute(idRoute).GetRoute();
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



	public static ListeArcs createMap(List<List<int>> argumentsArcs)
	{
		ListeArcs map;
		List<Arc> arcs = new List<Arc>();
		foreach (var list in argumentsArcs)
		{
			Arc arcTMP = new Arc(list[0], list[1], list[2], list[3], list[4], -1);
			arcs.Add(arcTMP);
		}

		map = new ListeArcs(arcs);
		return map;
	}

	public static List<List<int>> GetRails()
	{
		List<List<int>> rails = new List<List<int>>();
        string connectionString = "Server=192.168.100.102;Database=LesAventuriersDuRail;UserId=ghait;Password=mdpDeOuf_2024;";

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			connection.Open();

			MySqlCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Rails";

			using (MySqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					List<int> rail = new List<int>
					{
						reader.GetInt32("idRails"),
						reader.GetInt32("idVille1"),
						reader.GetInt32("idVille2"),
						reader.GetInt32("idClr"),
						reader.GetInt32("nbrDeRail")
					};
					rails.Add(rail);
				}
			}
		}

		return rails;
	}
}