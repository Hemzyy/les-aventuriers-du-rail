using MySql.Data.MySqlClient;
using System.Data;
public class CarteWagon : Carte
{
	// classe qui implemente la classe abstraite carte
	// elle represente une carte wagon

	// entier representant une couleur (comme stocke sur la base)
	public string Couleur { get; private set; }


	private static DataTable allCartes = chargerDonees();

	// le constructeur qui permet de produire une carte wagon (de couleur aleatoire) depuis la base de donnees
	public CarteWagon()
	{
		Random rnd = new Random();
		int randomNumber = rnd.Next(0, allCartes.Rows.Count);
		DataRow row = allCartes.Rows[randomNumber];

		// les informations utilise a partir du retour de la fonction chargerDonnees (depuis le dictionnaire row)
		Identifiant = (int)row["Identifiant"];
		Couleur = row["Couleur"].ToString();
	}


	// fonction qui permet de charger les donnees relatives a une carte wagon depuis la carte de donnees 
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
			using (MySqlCommand command = new MySqlCommand("select W.idCarte as Identifiant, C.nomClr as Couleur from cartesWagon W join Couleur C on W.idClr = C.idClr;select W.idCarte as Identifiant, C.nomClr as Couleur from cartesWagon W join Couleur C on W.idClr = C.idClr;", connection))
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


	// fonction permettant de transformer en string une carte wagon
	public override string ToString()
	{
		return $"{base.ToString()}, Couleur={Couleur}";
	}
}
