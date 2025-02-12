using MySql.Data.MySqlClient;
using System.Data;

public class CarteMission : Carte
{
    // classe qui implemente la classe abstraite Carte
    // cette classe represente une carte mission 

    // identifiant inherited de la class carte

    // entier qui indique la ville de depart de la carte mission en question (il est stocke comme un entier sur la base de donne)
    public string NomVilleDepart { get; private set; } = "";

    public int VilleDepart { get; private set; } = -1;

    // entier qui indique la ville d'arrive de la carte mission en question (il est stocke comme un entier sur la base de donne)
    public string NomVilleArrive { get; private set; } = "";

    public int VilleArrive { get; private set; } = -1;

    // entier qui indique le nombre de point qu'une carte mission va donner si accomplie, ou retire si non accomplie
    public int Points { get; private set; } = -1;


    // raw data representant toutes les Cartes mission sur la base de donnees (qu'on va charger)
    private static DataTable allCartes = chargerDonees();

    // permet de savoir si la mission a ete accomplie ou non
    public bool Accomplie { get; set; } = false;


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
                VilleDepart = (int)row["idVilleDepart"];
                NomVilleDepart = (string)row["VilleDepart"];
                VilleArrive = (int)row["idVilleArrive"];
                NomVilleArrive = (string)row["VilleArrive"];
                Points = (int)row["Points"];
                break;
            }
        }
    }

    // fonction qui doit charge toutes les Cartes missions dans un datatable (une sorte de datagram)
    private static DataTable chargerDonees()
    {
        // connection string avec les elements de connexion necessaires 
        string connectionString = "Server=192.168.100.102;Database=LesAventuriersDuRail;UserId=ghait;Password=mdpDeOuf_2024;";
        DataTable data = new DataTable();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            // requete sql permettant de recuperer toutes les Cartes de la table cartesMission
            // j'ajoute une colonne visited pour me permettre par la suite de voir si j'ai deja produit une carte mission ou pas (unicite de carte mission)
            using (MySqlCommand command = new MySqlCommand("SELECT m.idMission AS Identifiant, v1.idVille AS idVilleDepart, v1.NomVille AS VilleDepart, v2.idVille AS idVilleArrive, v2.NomVille AS VilleArrive, m.nbrDePoints AS Points, 0 AS visited FROM cartesMission m JOIN Ville v1 ON m.idVille1 = v1.idVille JOIN Ville v2 ON m.idVille2 = v2.idVille;", connection))
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
    
    public static void resetVisited()
    {
        // Modify the "Age" column values
        foreach (DataRow row in allCartes.Rows)
        {
            // Example: Increase age by 1
            row["visited"] = 0;
        }
    }
}
