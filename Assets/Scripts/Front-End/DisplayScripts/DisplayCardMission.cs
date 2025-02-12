using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


//Script lié à chaque instance de carte mission afin de changer son affichage
public class DisplayCardMission : MonoBehaviour
{
    public CardMission card;
    public string station1;
    public string station2;
    public int points;
    public bool selected;

    // Update the visual representation of the card
    public void UpdateCardVisuals(string s1, string s2, int pts)
    {
        selected = false;
        station1 = s1;
        station2 = s2;
        points = pts;

        //Update texts
        TextMeshProUGUI[] cardTexts = GetComponentsInChildren<TextMeshProUGUI>();
        if (cardTexts.Length >= 3){
            //La station 1
            cardTexts[0].text = s1;
            //La station 2
            cardTexts[1].text = s2;
            //Score de la carte
            cardTexts[2].text = pts.ToString();
        }
        else{
             Debug.LogWarning("Not enough TextMeshProUGUI components found in children of CardMission.");
        }
    }

    //Le joueur pioche une carte mission
    public void  SelectFromPioche()
    {
        Image[] cardImages = transform.GetComponentsInChildren<Image>();

        //Ajoute à la liste de séléction
        if(selected == false){
            selected = true;
            cardImages[0].color = Color.yellow;
        }
        else{
            selected = false;
            cardImages[0].color = Color.white;
        } 
    }
}
