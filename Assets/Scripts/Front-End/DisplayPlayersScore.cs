using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayersScore : MonoBehaviour
{
    public GameObject PlayerScorePrefab;

    Color[] colors = {Color.red, Color.green, Color.blue, Color.yellow};
    void Start()
    {
        DisplayPlayers();
    }

    public void DisplayPlayers(){
        Client client = Client.Instance;
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for(int i=0; i < client.nomJoueursDansLeLobby.Count ; i++) 
        {
            GameObject playerdisplay = Instantiate(PlayerScorePrefab, new Vector2(0, 0), Quaternion.identity);
            TextMeshProUGUI[] texts = playerdisplay.GetComponentsInChildren<TextMeshProUGUI>();
            Image[] playerColor = playerdisplay.GetComponentsInChildren<Image>();
            
            if(texts != null && playerColor != null){
                texts[0].text = client.nomJoueursDansLeLobby[i]; 
                texts[1].text = client.scoreJoueur[i];
                playerColor[1].color = colors[i];//= GameManager.Instance.playercolors[i]; 
            }
            else{
                Debug.LogWarning("PlayDisplay components not found");
            }

            playerdisplay.transform.SetParent(transform, false);
        }
    }

}
