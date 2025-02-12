using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DealCards : MonoBehaviour
{
    //public Transform PlayerHand;
    public InstanciateCards displayer;
    public InstanciateCards showMissionBtn;
    public InstanciateCards displayerWagonDraw;
    public GameObject MissionChoiceForm;
    InstanciateCards MissionFormInstanciate;
    public GameObject FeedBackMessageDisplay;

    void Awake()
    {
        Transform PlayerHand = transform.parent.Find("PlayerHand");
        displayer = PlayerHand.GetComponent<InstanciateCards>();
        Transform PlayerMission = transform.parent.Find("Btn_showPlayerMissions");
        showMissionBtn = PlayerMission.GetComponent<InstanciateCards>();
        Transform WagonDraw = transform.parent.Find("WagonDraw");
        displayerWagonDraw = WagonDraw.GetComponent<InstanciateCards>();

        if(MissionChoiceForm != null){
            MissionChoiceForm.SetActive(false);    
            
            MissionFormInstanciate = MissionChoiceForm.GetComponentInChildren<InstanciateCards>();
            if(MissionFormInstanciate == null)
                Debug.LogError("MissionForm not found");
        }
    }

    //Distribution des cartes
    public void DealFourTrainCards()
    {
        //Tire 4 cartes dans la main du joueur
        GameAudioManger.Instance.PlayCards();
        DealRandomCards(GameManager.Instance.GetCurrentPlayer(), 4);
        displayer.DisplayWagons(GameManager.Instance.GetCurrentPlayer().hand, 0);
    }

    public void DrawOneTrainCard()
    {
        //-------------------------reçoit id de couleur 1 = rouge ... -> faire -1
        //int idCard = 
        //GameManager.Instance.GetCurrentPlayer().hand[idCard].count += 1;
        if(Client.Instance.myTurn)
        {
            FeedBackMessageDisplay.GetComponent<FeedBackMessage>()
                .DisplayMessage("A vous de jouer !!", Color.green);
            
            Player cplayer = GameManager.Instance.GetCurrentPlayer();
            if (cplayer.action == GameManager.PlayerAction.Nothing ||
                cplayer.action == GameManager.PlayerAction.PiocheWagon)
            {
                GameManager.Instance.GetCurrentPlayer().action = GameManager.PlayerAction.PiocheWagon;
                int id_carte = DealRandomCards_id(GameManager.Instance.GetCurrentPlayer());
                GameManager.Instance.CartesRestantentAPiocherPendantCeTour--;
                displayer.DisplayWagons(GameManager.Instance.GetCurrentPlayer().hand, 0);

                string info = "10006|" + id_carte.ToString();
                Client.Instance.Send(info,Client.Instance.socket);
            }
            else
            {
                FeedBackMessageDisplay.GetComponent<FeedBackMessage>()
                    .DisplayMessage("Veuillez finir l'action en cours.", Color.red);
            }
        }
        else
        {
            FeedBackMessageDisplay.GetComponent<FeedBackMessage>()
                .DisplayMessage("Ce n'est pas votre tour !! ", Color.red);
            
        }
    }
    

    //Ajoute à une main initialisé : n cartes
    public void DealRandomCards(Player player, int n)
    {
        GameAudioManger.Instance.PlayCards();
        for(int i=0; i < n; i++){
            int nb = Random.Range(0, GameManager.Instance.NumberOfColors);
            player.hand[nb].count += 1;
        }
    }
    public int DealRandomCards_id(Player player)
    {
        int nb = Random.Range(0, GameManager.Instance.NumberOfColors);
        player.hand[nb].count += 1;
        return nb+1;
    }

    public void DisplayMissionForm()
    {
        //-------------------------recoit liste de liste de 4 strings (id, s1, s2, ponits) pour les 3 cartes
        //-------------------------les créer
        //List<List<string>> packet = new List<List<string>>() = 
        if(Client.Instance.myTurn)
        {
            GameAudioManger.Instance.PlayButtonClickSound();
            Player cplayer = GameManager.Instance.GetCurrentPlayer();
            if(cplayer.action == GameManager.PlayerAction.Nothing){
                GameManager.Instance.GetCurrentPlayer().action = GameManager.PlayerAction.PiocheMission;
                GameManager.Instance.InitMissionDraw();  //packet);
                MissionFormInstanciate.DisplayPlayerMissions(GameManager.Instance.PiocheMissionList, MissionFormInstanciate.PlayerMissionContainer.transform);
                MissionChoiceForm.SetActive(true);
            }
            else if(cplayer.action == GameManager.PlayerAction.PiocheMission){
                GameAudioManger.Instance.PlayErrorSound();
                FeedBackMessageDisplay.GetComponent<FeedBackMessage>().DisplayMessage("Choisissez une carte mission!", Color.red);
            }
            else{
                GameAudioManger.Instance.PlayErrorSound();
                FeedBackMessageDisplay.GetComponent<FeedBackMessage>().DisplayMessage("Veuillez finir l'action en cours.",Color.red);
            }
        }
        else{

            GameAudioManger.Instance.PlayErrorSound();
            FeedBackMessageDisplay.GetComponent<FeedBackMessage>().DisplayMessage("Ce n'est pas à vous de jouer.",Color.red);
        }
    }
}
