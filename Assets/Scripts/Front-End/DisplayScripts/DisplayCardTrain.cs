using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Jobs;
using UnityEngine.UI;


//Script lié à chaque instance de carte train afin de changer son affichage et la rendre intéractive
public class DisplayCardTrain : MonoBehaviour
{
    public CardTrain card;
    public string cardSymbole;
    public int cardN;
    public bool isSelected;
    public static int decalage = 40;

    public void Start()
    {
        isSelected = false;

    }

    //Joueur selectionne depuis sa main
    public void  Select()
    {
        if(Client.Instance.myTurn)
        {
            Player cplayer = GameManager.Instance.GetCurrentPlayer();
            if(cplayer.action == GameManager.PlayerAction.Nothing ){
                if(!isSelected)
                {
                    ResetCardsPosition(transform.parent);
                    isSelected = true;
                    transform.Translate(Vector3.up * 1);
                    
                    Player player = GameManager.Instance.GetCurrentPlayer();
                    player.selection = cardSymbole;
                    player.selectionSize = cardN;
                }
            }
            else{
                transform.parent.parent.Find("FeedBackMessages").GetComponent<FeedBackMessage>().DisplayMessage("Vous êtes en train de piocher.",Color.red);
            }
        }
        else{
                transform.parent.parent.Find("FeedBackMessages").GetComponent<FeedBackMessage>().DisplayMessage("Ce n'est pas à vous de jouer.",Color.red);
        }
    }

    //Le joueur passe sa sourie sur les cartes de sa main
    public void MouseEnterFromHand()
    {
        if(isSelected == false){
            transform.Translate(Vector3.up * decalage);
        }    
    }
    public void MouseExitFromHand()
    {
        if(isSelected == false){
            transform.Translate(new Vector3(0,-decalage,0));
        }      
    }

    //Reset la position des cartes lors de la séléction
    public void ResetCardsPosition(Transform parent)
    {
        foreach (Transform child in parent)
        {
            DisplayCardTrain cardDisplay = child.GetComponent<DisplayCardTrain>();
            if(cardDisplay.isSelected == true){
                cardDisplay.isSelected = false;
                child.Translate(new Vector3(0,-decalage,0));
            }
        }
    }



    //Le joueur pioche une carte
    public void  SelectFromDraw()
    {
        if(Client.Instance.myTurn)
        {
            Player cplayer = GameManager.Instance.GetCurrentPlayer();

            if(cplayer.action == GameManager.PlayerAction.Nothing || cplayer.action == GameManager.PlayerAction.PiocheWagon){
                cplayer.action = GameManager.PlayerAction.PiocheWagon;

                List<CardTrain> playerhand = GameManager.Instance.GetCurrentPlayer().hand;
                //player hand add card
                for(int i=0; i < playerhand.Count; i++){
                    if(playerhand[i].symbole == cardSymbole)
                        playerhand[i].count++;
                }
                //update affichage main
                Transform handDisplayer = transform.parent.parent.Find("PlayerHand");
                if(handDisplayer != null)
                    handDisplayer.GetComponent<InstanciateCards>().DisplayWagons(playerhand, 0);
                else
                    Debug.Log("handDisplayer not found.");
                    
                //change la carte dans la pioche
                List<CardTrain> piochewagon = GameManager.Instance.PiocheWagonList;
                int j = 0;
                foreach (Transform child in transform.parent)
                {
                    if(child == transform){

						/*// ici on doit faire la demande d'envoie 
                        Client.Instance.Send($"10010|{j}", Client.Instance.socket);
                        // comment est ce que je peux le retarder ici
                        int idWagon = int.Parse(Client.Instance.case20010[0][0]);
                        idWagon = int.Parse(Client.Instance.case20010[0][0]);
						CardTrain card = new CardTrain(GameManager.Instance.CardTrainList[idWagon - 1].color, GameManager.Instance.CardTrainList[idWagon - 1].symbole);
						card.count = 1;
						piochewagon[j] = card; //chope  la carte du server pour recevoir une nouvelle carte wagon*/

                        Client.Instance.Send($"10011|{j}", Client.Instance.socket);

						//piochewagon[j] = GameManager.Instance.GetRandomCardTrain(); //chope  la carte du server pour recevoir une nouvelle carte wagon
						break;
					}
                    j++;
                }
                GameManager.Instance.CartesRestantentAPiocherPendantCeTour--;
                transform.parent.GetComponent<InstanciateCards>().DisplayWagons(piochewagon, 0);
            }
            else{
                transform.parent.parent.Find("FeedBackMessages").GetComponent<FeedBackMessage>().DisplayMessage("Veuillez finir l'action en cours.",Color.red);
            }
        }
        else{
                transform.parent.parent.Find("FeedBackMessages").GetComponent<FeedBackMessage>().DisplayMessage("Ce n'est pas votre tours",Color.red);
        }
    }

    //Le joueur passe sa sourie au dessus de la pioche wagon visible
    public void MouseEnterFromDraw()
    {
        transform.Translate(Vector3.left * decalage);
    }
    public void MouseExitFromDraw()
    {
        transform.Translate(Vector3.left * -decalage);
    }

    // Update the visual representation of the card
    public void UpdateCardVisuals(Color32 col, string sym, int c)
    {
        cardSymbole = sym;
        cardN = c;
        //Update color
        Image[] cardImages = GetComponentsInChildren<Image>();
        if (cardImages.Length > 1){
            cardImages[1].color = col;
        }
        else{
            Debug.LogWarning("Not enough Image components found in children.");
        }

        //Update texts
        TextMeshProUGUI[] cardTexts = GetComponentsInChildren<TextMeshProUGUI>();
        if (cardTexts.Length >= 2){
            //Le nombre de cartes
            if(c >= 2)
                cardTexts[0].text = c.ToString();
            else
                cardTexts[0].text = "";
            //Le symbole de la carte
            cardTexts[1].text = sym;
        }
        else{
             Debug.LogWarning("Not enough TextMeshProUGUI components found in children.");
        }
    }
}
