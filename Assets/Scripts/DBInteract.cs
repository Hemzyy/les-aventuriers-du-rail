using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using TMPro;

public class DBInteract : MonoBehaviour
{
    private MySqlConnectionStringBuilder stringBuilder;

    private string Server = "127.0.0.1";
    private string Database = "LesAventuriersDuRail";
    private string UserID = "hamza";
    private string Password = "hamza";
    public TextMeshProUGUI errorField;

    // Start is called before the first frame update
    void Start()
    {
        stringBuilder = new MySqlConnectionStringBuilder();
        stringBuilder.Server = Server;
        stringBuilder.Database = Database;
        stringBuilder.UserID = UserID;
        stringBuilder.Password = Password;
    }

    public bool InsertPlayer(string pseudo, string motDePass)
    {
        bool res = false;
        using (MySqlConnection connection = new MySqlConnection(stringBuilder.ConnectionString))
        {
            try
            {
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Joueur (pseudo, motDePass) VALUES (@pseudo, @motDePass)";
                command.Parameters.AddWithValue("@pseudo", pseudo);
                command.Parameters.AddWithValue("@motDePass", motDePass);

                command.ExecuteNonQuery();
                connection.Close();
                res = true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("DBInteract: Could not add the player! " + System.Environment.NewLine + ex.Message);
                errorField.SetText("DBInteract: Could not add the player! " + System.Environment.NewLine + ex.Message);
                res = false;
            }
        }
        return res;
    }

    // when a user wants to connect, we check if the username and password are correctly in the database. if username doesn't exist or password doesn't match, we return console.log("Username or password incorrect")
    public bool login(string pseudo, string motDePass)
    {
        bool res = false;
        using (MySqlConnection connection = new MySqlConnection(stringBuilder.ConnectionString))
        {
            try
            {
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Joueur WHERE pseudo = @pseudo AND motDePass = @motDePass";
                command.Parameters.AddWithValue("@pseudo", pseudo);
                command.Parameters.AddWithValue("@motDePass", motDePass);

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Debug.Log("Username and password are correct");
                    res = true;
                }
                else
                {
                    Debug.Log("Username or password incorrect");
                }

                connection.Close();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("DBInteract: Could not check the player! " + System.Environment.NewLine + ex.Message);
                errorField.SetText("DBInteract: Could not check the player! " + System.Environment.NewLine + ex.Message);
                res = false;
            }
        }
        return res;
    }

    //retrieve all columns line by line from the table rails
    public List<List<int>> GetRails()
    {
        List<List<int>> rails = new List<List<int>>();

        using (MySqlConnection connection = new MySqlConnection(stringBuilder.ConnectionString))
        {
            try
            {
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM rails";

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
            catch (System.Exception ex)
            {
                Debug.LogError("DBInteract: Could not get the rails! " + System.Environment.NewLine + ex.Message);
            }
        }

        return rails;
    }

}