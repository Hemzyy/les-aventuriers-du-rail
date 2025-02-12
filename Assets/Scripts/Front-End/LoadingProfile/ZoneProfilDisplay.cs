using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ZoneProfilDisplay : MonoBehaviour
{
    public GameObject[] playerAvatars;
    public TextMeshProUGUI[] playerNames;
    public string connecte = "est connecté";
    public void UpdatePlayerProfiles(string[] playerNames, Sprite[] playerAvatars)
    {
        // Mettre � jour les avatars des joueurs
        for (int i = 0; i < playerAvatars.Length; i++)
        {
            this.playerAvatars[i].GetComponent<SpriteRenderer>().sprite = playerAvatars[i];
        }

        // Mettre � jour les noms des joueurs
        for (int i = 0; i < playerNames.Length; i++)
        {
            this.playerNames[i].text = playerNames[i];
        }
    }
}
