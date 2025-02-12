using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayersInLobby : MonoBehaviour
{

    public GameObject PlayerPrefab;
    Color[] colors = {Color.red, Color.green, Color.blue, Color.yellow};


    void Update()
    {
        DisplayPlayers();
    }

    public void DisplayPlayers(){
        Client client = Client.Instance;
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for(int i=0; i < client.nomJoueursDansLeLobby.Count; i++)
        {
            GameObject playerdisplay = Instantiate(PlayerPrefab, new Vector2(0, 0), Quaternion.identity);
            TextMeshProUGUI playerNameText = playerdisplay.GetComponentInChildren<TextMeshProUGUI>();
            Image[] playerColor = playerdisplay.GetComponentsInChildren<Image>();
            
            if(playerNameText != null && playerColor != null){
                playerNameText.text = client.nomJoueursDansLeLobby[i]; //récupérer le nom du joueur de la liste du lobby
                playerColor[1].color = colors[i];
            }
            else{
                Debug.LogWarning("PlayDisplay components not found");
            }

            playerdisplay.transform.SetParent(transform, false);
        }
    }
}
