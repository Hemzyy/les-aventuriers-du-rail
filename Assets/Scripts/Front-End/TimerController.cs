using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Image timerFilling;
    public float time_remaining;
    public float max_time = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        time_remaining = max_time;
    }

    // Update is called once per frame
    void Update()
    {
        //time_remaining > 0 &&
        if( time_remaining > 0 && GameManager.Instance.CartesRestantentAPiocherPendantCeTour > 0){
            time_remaining -= Time.deltaTime;
            timerFilling.fillAmount = time_remaining / max_time;
        }
        else{
            //GameManager.Instance.NextTurn();
            //timerFilling.color = GameManager.Instance.GetCurrentPlayer().playerColor;
            //time_remaining = max_time;
            if(Client.Instance.myTurn){
                GameManager.Instance.NextTurn();
            }
        }
    }
}
