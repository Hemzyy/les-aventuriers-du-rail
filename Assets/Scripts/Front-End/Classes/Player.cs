using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string playerName;
    public Color playerColor;
    public List<CardTrain> hand;
    public List<CardMission> missions;
    public string selection;
    public int selectionSize;
    public int wagonCount;
    public GameManager.PlayerAction action;


    public Player(){}
    public Player(string name, Color color)
    {
        playerName = name;
        playerColor = color;
        selection = "";
        selectionSize = 0;
        wagonCount = 15;
        hand = new List<CardTrain>();
        missions = new List<CardMission>();
        action = GameManager.PlayerAction.Nothing;
    }

    public void ResetPlayerSelection()
    {
        selection = "";
        selectionSize = 0;
    }

    public void UsePlayerCard(int routeLength)
    {
        for(int i=0; i<hand.Count; i++){
            if(hand[i].symbole == selection){
                hand[i].count -= routeLength;
                break;
            }
        }
        //wagonCount -= routeLength;
        ResetPlayerSelection();
    }
}
