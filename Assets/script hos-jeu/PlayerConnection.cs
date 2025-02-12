using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PlayerConnection : MonoBehaviour
{
    private Client client;
    public TMP_InputField username_a;
    public TMP_InputField password_a;
    public TextMeshProUGUI errorField;
    public TMP_InputField code_du_lobby_a_rejoindre; // ici on stocke le code du lobby à rejoindre

    void Start()
    {
        client = Client.Instance;
        if (client == null)
        {
            Debug.LogError("Client instance not found.");
        }
    }
    

    public void RegisterOnClick()
    {
        string username = username_a.text;
        string password = password_a.text;
        if (IsValid(username, password))
        {
            PersistentAudioManager.Instance.PlayButtonClickSound();
            string senddata = "99999|" + username + "|" + password;
            client.Send(senddata, client.socket);
        }
        StartCoroutine(CheckConnectionAndLoadScene());
        if(!client.connectedABDD)
        {
            // If not connected, show error message
            
        }
    }

    public void LancerPartie()
    {
        if (client.chefDuLobby)
        {
            PersistentAudioManager.Instance.PlayButtonClickSound();
            Debug.Log("le chef de partie lance");
            client.Send("99994",client.socket);
            SceneManager.LoadScene("Game");
        }
        else
        {
            PersistentAudioManager.Instance.PlayErrorMessage();
            errorField.SetText("Vous ne pouvez pas lancer la partie.");
            Debug.Log("pas le droit");
        }
    }
    public void LoginButton()
    {
        PersistentAudioManager.Instance.PlayButtonClickSound();
        string username = username_a.text;
        string password = password_a.text;
        if (IsValid(username, password))
        {
            string senddata = "99998|" + username + "|" + password;
            client.Send(senddata, client.socket);
        }
        
        StartCoroutine(CheckConnectionAndLoadScene());
        if(!client.connectedABDD)
        {
            // If not connected, show error message
            //errorField.text = "pseudo ou mdp faux.";
            Debug.Log("pseudo ou mdp faux");
        }
    }
    private IEnumerator CheckConnectionAndLoadScene()
    {
        float timeout = 0.5f; // Timeout after 1 seconds
        float timer = 0f;

        // Wait until the client is connected or the timeout is reached
        while (!client.connectedABDD && timer < timeout)
        {
            yield return new WaitForSeconds(0.1f); // Wait for 100 milliseconds before checking again
            timer += 0.1f;
        }

        if (client.connectedABDD)
        {
            // Now that we're connected, load the scene
            SceneManager.LoadScene("Account");
        }
        else{
            PersistentAudioManager.Instance.PlayErrorMessage();
            errorField.text = "L'INSCRIPTION / LA CONNEXION À ÉCHOUÉ.";
        }
    }
    
    private IEnumerator CheckLobbyAndLoadScene()
    {
        float timeout = 0.5f; // Timeout after 1 seconds
        float timer = 0f;

        // Wait until the client is connected or the timeout is reached
        while (!client.LobbyExists && timer < timeout)
        {
            yield return new WaitForSeconds(0.1f); // Wait for 100 milliseconds before checking again
            timer += 0.1f;
        }

        if (client.LobbyExists)
        {
            // Now that we're connected, load the scene
            SceneManager.LoadScene("LoadingScreen");
        }
        else{
            PersistentAudioManager.Instance.PlayErrorMessage();
            errorField.text = "CE LOBBY N'EXISTE PAS.";
        }
    }


    public bool IsValid(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            PersistentAudioManager.Instance.PlayErrorMessage();
            errorField.SetText("Veuillez remplir les champs.");
            return false;
        }
        return true;
    }
    

    public void CreerUnePartie() // ici c'est le code pour Creer un lobby
    {
        PersistentAudioManager.Instance.PlayButtonClickSound();
        string actionCreer;
        actionCreer ="0";
        client.Send(actionCreer, client.socket);
        //Recevoir le code du lobby
        SceneManager.LoadScene("LoadingScreen");
    }

    public void RejoindreUnePartieScene()
    {
        PersistentAudioManager.Instance.PlayButtonClickSound();
        SceneManager.LoadScene("PartyForm");
    }

    public void RejoindreUnePartie() // ici c'est le code pour rejoindre un lobby (qui existe déjà)
    {
        string codeLobby;
        codeLobby ="1|"+ code_du_lobby_a_rejoindre.text;
        
        client.Send(codeLobby, client.socket);
        //bool client.LobbyExists = true; //Response du server si code bon / lobby existe
        //client.cs has a bol that needs to be checked
        StartCoroutine(CheckLobbyAndLoadScene());
        
        if (client.LobbyExists == false)
        {
            //errorField.SetText("Le lobby n'existe pas.");
        }
        
    }
}
