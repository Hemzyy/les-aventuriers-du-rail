using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void GoBackToAccount(){
        PersistentAudioManager.Instance.PlayButtonClickSound();
        SceneManager.LoadScene("Account");
    }

    public void GoBackToMainMenu(){

        PersistentAudioManager.Instance.PlayButtonClickSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void GoaBackToConnectForm(){

        PersistentAudioManager.Instance.PlayButtonClickSound();
        SceneManager.LoadScene("ConnectForm");
    }

    public void DisconnectFromLobby(){
        Client client = Client.Instance;
        client.Send("2", client.socket);
        SceneManager.LoadScene("Account");
    }

    public void QuitGame(){
        Client client = Client.Instance;
        client.Send("2", client.socket);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OpenOption(){
        SceneManager.LoadScene("Options");
    }
}
