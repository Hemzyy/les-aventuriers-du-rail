using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameForm : MonoBehaviour
{
    public Button button;
    public TMP_InputField input;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(JoinGame);
        }
        else
        {
            Debug.LogError("le bouton n'est pas assigné");
        }

    }

    public void JoinGame()
    {
        input = GetComponentInChildren<TMP_InputField>();

        string input_a = input.text;

        if (IsValid(input_a))
        {
            Debug.Log("you're joining a game");
            SceneManager.LoadScene("LoadingScreen");
        }
        else
        {
            Debug.LogWarning("incorrect :  JoinGame");
        }

    }

    public bool IsValid(string username)
    {
       
        return true;
    }


}
