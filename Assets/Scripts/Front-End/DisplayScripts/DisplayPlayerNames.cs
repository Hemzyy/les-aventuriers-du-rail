using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DisplayPlayerNames : MonoBehaviour
{
    public GameObject PlayerNameDisplayPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayPlayers(List<Player> players){
        ClearChildren(transform);

        for(int i=0; i < players.Count; i++)
        {
            GameObject playerdisplay = Instantiate(PlayerNameDisplayPrefab, new Vector2(0, 0), Quaternion.identity);
            bool hisTurn = false;
            if(GameManager.Instance.CurrentPlayerIndex == i)
                hisTurn = true;
            UpdateVisuals(playerdisplay, players[i].playerName, players[i].playerColor, players[i].wagonCount, hisTurn);
            playerdisplay.transform.SetParent(transform, false);
        }
    }

    public void UpdateVisuals(GameObject display, string name, Color col, int nb_wagons, bool hisTurn)
    {
        TextMeshProUGUI[] displayTexts = display.GetComponentsInChildren<TextMeshProUGUI>();
        if (displayTexts.Length >= 3){
            //Le nom du joueur
            displayTexts[0].text = name;
            //Le nombre de wagon
            displayTexts[1].text = nb_wagons.ToString();
            //Indicateur du jour en cours
            if(hisTurn == true)
                displayTexts[2].text = ">";
            else
                displayTexts[2].text = "";
        }
        else{
             Debug.LogWarning("Not enough TextMeshProUGUI components found in children of display.");
        }
        UnityEngine.UI.Image displayImage = display.GetComponent<UnityEngine.UI.Image>();
        displayImage.color = col;
        //else{
        //     Debug.LogWarning("Display Image not fount");
        //}
    }

    //Supprime toutes les instances des enfants d'un parent
    public void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
