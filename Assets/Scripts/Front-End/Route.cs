using System.Collections;
using System.Collections.Generic;
using clientPackage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Route : MonoBehaviour
{
    public int idRoute;
    public int idStation1;
    public int idStation2;
    public string nomStation1;
    public string nomStation2;
    public int idColor;
    public Color color;
    public int nbrTiles;
    bool isCaptured;
    string tileSymbole;
    FeedBackMessage FeedBackText;

    void Awake()
    {
        //récupère le symbole de la case au réveille
        TextMeshProUGUI tileText = transform.GetComponentInChildren<TextMeshProUGUI>();
        if (tileText != null){
            tileSymbole = tileText.text;
        }
        else{
            Debug.LogWarning("Tile text not found.");
        }
        isCaptured = false;
    }
    void Start()
    {
        FeedBackText = transform.parent.parent.parent.parent.Find("FeedBackMessages").GetComponent<FeedBackMessage>();
        if(FeedBackText == null)
            Debug.LogError("FeedbackText not found");
    }

    //Appelé quand le joueur clique sur une route
    public void OnClick()
    {
        //Debug.Log("Route cliqué: "+idRoute.ToString()+" "+tileSymbole);
        if(Client.Instance.myTurn)
        {
            Player player = GameManager.Instance.GetCurrentPlayer();
            //La selection du joueur est non nul
            if(!string.IsNullOrEmpty(player.selection))
            {
                //La route n'est pas déjà capturé
                if(!isCaptured)
                {
                    //Séléction du joueur = case
                    if(tileSymbole == player.selection)
                    {
                        //Le joueur a assé de cartes séléctionnées
                        if(player.selectionSize >= nbrTiles)
                        { 
                            CaptureRoute(transform, player.playerColor);
                            //Reset la séléction du joueur et retire ses cartes utilisées
                            player.UsePlayerCard(nbrTiles);
                            transform.parent.parent.parent.parent.Find("PlayerHand").GetComponent<InstanciateCards>().DisplayWagons(player.hand, 0);
                            FeedBackText.DisplayMessage("Route construite: "+nomStation1+" - "+nomStation2+" "+tileSymbole+" "+nbrTiles, Color.green);

                            //------------------------- envoie id de la route au serv!!
                            //
                            //PaquetMessage pm = new PaquetMessage(Client.Instance.idPlayerLobby, 10040, new List<List<string>>{new List<string> {idRoute.ToString()}});
                            Client.Instance.Send($"10040|{idRoute}|{Client.Instance.idPlayerLobby}", Client.Instance.socket);

                            GameManager.Instance.NextTurn();
                        }
                        else{
                            FeedBackText.DisplayMessage("Vous avez besoin de "+nbrTiles+" cartes pour construire cette route!", Color.red);
                        }
                    }
                    else{
                        FeedBackText.DisplayMessage("Sélectionnez la couleur correspondante! "+tileSymbole+" != "+player.selection, Color.red);
                    }
                }
                else{
                    FeedBackText.DisplayMessage("Cette route est déjà prise!", Color.red);   
                }
            }
            else{
                FeedBackText.DisplayMessage("Sélectionnez une carte pour construire une route!", Color.red);
            }
        }
        else{
            FeedBackText.DisplayMessage("Votre tour est passé!", Color.red);
        }

    }
    
    //Change la couleur de la route en fonction du joueur qui là capturé
    public void CaptureRoute(Transform route, Color playercolor)
    {
        isCaptured = true;
        foreach (Transform child in route)
        {
            //Images
            Image[] images = child.GetComponentsInChildren<Image>();
            if (images.Length >= 2){
                Color color = images[1].color;
                color.a = 0f;
                images[0].color = playercolor;
                images[1].color = color;
            }
            else{
                Debug.LogWarning("tile images not found.");
            }
            
            //Text
            TextMeshProUGUI tileSymbole = child.GetComponentInChildren<TextMeshProUGUI>();
            if (tileSymbole != null){
                //Tile symbole
                tileSymbole.text = "";
            }
            else{
                Debug.LogWarning("tile symbole not found.");
            }
        }
    }

}
