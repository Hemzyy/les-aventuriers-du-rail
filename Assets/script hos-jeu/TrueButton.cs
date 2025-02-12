using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrueButton : MonoBehaviour
{
    public Button button;
    public string scene;
    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            // Ajoutez un �couteur d'�v�nements au bouton
            button.onClick.AddListener(OnClick);
        }
        else
        {
            Debug.LogError("Le bouton n'est pas assign� dans l'inspecteur.");
        }


    }

    public void OnClick()
    {
        PersistentAudioManager.Instance.PlayButtonClickSound();
        string sceneName = scene.ToString();
        SceneManager.LoadScene(sceneName);
    }
}
