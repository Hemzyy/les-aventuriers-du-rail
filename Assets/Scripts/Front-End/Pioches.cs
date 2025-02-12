using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pioches : MonoBehaviour
{
    public GameObject PiocheMission;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickOnPiocheMissionConfirm()
    {
        Transform cardcontainer = transform.parent.Find("CardContainer");
        int i = 0;
        Player player = GameManager.Instance.GetCurrentPlayer();

        foreach(Transform card in cardcontainer){
            DisplayCardMission displaycard = card.GetComponent<DisplayCardMission>();
            if(displaycard.selected){

                //Ajout de la carte au joueur et retrait de la pioche   
                player.missions.Add(GameManager.Instance.PiocheMissionList[i]);
                
                //------------------------Appeler la fonction envois id des cartes choisis
                int id = GameManager.Instance.PiocheMissionList[i].id;
                //Send(id)
                GameManager.Instance.CardMissionList.Remove(GameManager.Instance.PiocheMissionList[i]);
                GameManager.Instance.NumberOfMissions--;
                Debug.Log(displaycard.station2+" "+displaycard.station2);
            }
            i++;
        }
        
        //Ferme la pioche
        PiocheMission.SetActive(false);  
        GameManager.Instance.NextTurn();
    }
}
