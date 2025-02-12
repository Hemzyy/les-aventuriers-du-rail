using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FeedBackMessage : MonoBehaviour
{
    public float message_duration = 5.0f;
    public float time_remaining;
    public TextMeshProUGUI Message;
    public Color messageColor;
    public bool displaying;

    // Start is called before the first frame update
    void Start()
    {
        displaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(displaying){
            if(time_remaining > 0){
                time_remaining -= Time.deltaTime;
                messageColor.a = time_remaining / message_duration;
                Message.color = messageColor;
            }
            else{
                Message.SetText("");
                displaying = false;
            }
        }

    }
    public void DisplayMessage(string message_text, Color col){
        time_remaining = message_duration;  
        Message.SetText(message_text);
        messageColor = col;
        displaying = true;
    }
}
