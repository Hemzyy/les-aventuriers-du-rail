using MySql.Data.MySqlClient;
using System.Data;

public class CarteMission : Carte
{
	// classe qui implemente la classe abstraite Carte
	// cette classe represente une carte mission 
	
	// identifiant inherited de la class carte
	
	// entier qui indique la ville de depart de la carte mission en question (il est stocke comme un entier sur la base de donne)
	public string VilleDepart { get; private set; } = "-1";
	
	// entier qui indique la ville d'arrive de la carte mission en question (il est stocke comme un entier sur la base de donne)
	public string VilleArrive { get; private set; } = "-1";

	// entier qui indique le nombre de point qu'une carte mission va donner si accomplie, ou retire si non accomplie
	public int Points { get; private set; } = -1;


	// raw data representant toutes les Cartes mission sur la base de donnees (qu'on va charger)
	private static DataTable allCartes = chargerDonees();


	// constructeur d'une carte mission qui ne produit pas de doublant
	public CarteMission()
	{
		for (int i = 0; i < allCartes.Rows.Count; i++)
		{
			DataRow row = allCartes.Rows[i];
			int visited = Convert.ToInt32(row["visited"]);

			if (visited == 0)
			{
				row["visited"] = 1; // ca c'est pour indiquer qu'on a deja vu / produit cette carte. pas la peine d'y revenir
				Identifiant = (int)row["Identifiant"];
				VilleDepart = row["VilleDepart"].ToString();
				VilleArrive = row["VilleArrive"].ToString();
				Points = (int)row["Points"];
				break;
			}
		}
	}

	// fonction qui doit charge toutes les Cartes missions dans un datatable (une sorte de datagram)
	private static DataTable chargerDonees()
	{
		// connection string avec les elements de connexion necessaires 
		string connectionString = "Server=localhost;Port=3307;Database=LesAventuriersDuRail;Uid=ghait;Pwd=mdpDeOuf_2024;";
		DataTable data = new DataTable();

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			connection.Open();
			// requete sql permettant de recuperer toutes les Cartes de la table cartesMission
			// j'ajoute une colonne visited pour me permettre par la suite de voir si j'ai deja produit une carte mission ou pas (unicite de carte mission)
			using (MySqlCommand command = new MySqlCommand("select m.idMission as Identifiant, v1.NomVille as VilleDepart, v2.NomVille as VilleArrive, m.nbrDePoints as Points, 0 as visited from cartesMission m join Ville v1 on m.idVille1 = v1.idVille join Ville v2 on m.idVille2 = v2.idVille;", connection))
			{
				// Use a MySqlDataAdapter to fill the DataTable
				using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
				{
					adapter.Fill(data);
				}
			}
		}
		return data;
	}



	// fonction permettant de transformer en string une carte mission
	public override string ToString()
	{
		return $"{base.ToString()}, VilleDepart={VilleDepart}, VilleArrive={VilleArrive}, Points={Points}";
	}
}
