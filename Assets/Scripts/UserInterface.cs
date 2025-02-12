using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public InputField pseudo;
    public InputField motDePass;

    DBInteract DBInteract;

    // Start is called before the first frame update
    void Start()
    {
        DBInteract = FindObjectOfType<DBInteract>();
    }

    public void InsertPlayer()
    {
        if (DBInteract == null)
        {
            Debug.LogError("UserInterface: Could not insert a player. DBIinteract is not present.");
            return;
        }

        if (pseudo == null || motDePass == null)
        {
            Debug.LogError("UserInterface: Could not insert a player. pseudo or motDePass is not set.");
            return;
        }

        if (string.IsNullOrEmpty(pseudo.text) || string.IsNullOrWhiteSpace(pseudo.text))
        {
            Debug.LogError("UserInterface: Could not insert a highscore. PlayerName is empty.");
            return;
        }


        DBInteract.InsertPlayer(pseudo.text, motDePass.text);
        pseudo.text = "";
        motDePass.text = "";
    }

    public void login(){
        if (DBInteract == null)
        {
            Debug.LogError("UserInterface: Could not login. DBIinteract is not present.");
            return;
        }

        if (pseudo == null || motDePass == null)
        {
            Debug.LogError("UserInterface: Could not login. pseudo or motDePass is not set.");
            return;
        }

        if (string.IsNullOrEmpty(pseudo.text) || string.IsNullOrWhiteSpace(pseudo.text))
        {
            Debug.LogError("UserInterface: Could not login. PlayerName is empty.");
            return;
        }

        DBInteract.login(pseudo.text, motDePass.text);
        pseudo.text = "";
        motDePass.text = "";
    }

}