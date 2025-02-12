using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;
using System;

public class InstanciateCards : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject PlayerMissionsDisplayer;
    public GameObject PlayerMissionContainer;
    bool showMission;
    
    void Awake()
    {
        /*
        if( PlayerMissionsDisplayer != null ){
            PlayerMissionsDisplayer.SetActive(false);
            showMission = false;
        }
        */
        showMission = true;
    }
    public void DisplayWagons(List<CardTrain> hand, int angle)
    {
        //Supprime l'affichage précèdent de la main
        ClearChildren(transform);

        for(int i=0; i < hand.Count; i++)
        {
            if(hand[i].count > 0)
            {
                GameObject card = Instantiate(CardPrefab, new Vector2(angle, 0), Quaternion.identity);
                card.GetComponent<DisplayCardTrain>().UpdateCardVisuals(hand[i].color, hand[i].symbole, hand[i].count);
                card.transform.SetParent(transform, false);
            }
        }
    }

    public void DisplayPlayerMissions(List<CardMission> missions, Transform trfm)
    {
        //Supprime l'affichage précèdent de la main
        ClearChildren(trfm);

        for(int i=0; i < missions.Count; i++)
        {
            GameObject card = Instantiate(CardPrefab, new Vector2(0, 0), Quaternion.identity);
            card.GetComponent<DisplayCardMission>().UpdateCardVisuals(missions[i].station1, missions[i].station2, missions[i].points);
            card.transform.SetParent(trfm, false);
        }
    }

    public void ClickDisplayMissions()
    {
        if(showMission == true){
            PlayerMissionsDisplayer.SetActive(false);
            showMission = false;
        }
        else{
            DisplayPlayerMissions(GameManager.Instance.GetCurrentPlayer().missions, PlayerMissionContainer.transform);
            PlayerMissionsDisplayer.SetActive(true);
            showMission = true;
        } 
    }


    //Supprime toutes les instances des enfants d'un parent
    public void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void UpdatePlayerHand(List<CardTrain> hand){
        //GameManager.Instance.GetCurrentPlayer().UsePlayerCard();
        DisplayWagons(hand, 0);
    }
}
