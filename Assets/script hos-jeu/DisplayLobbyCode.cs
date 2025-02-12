using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayLobbyCode : MonoBehaviour
{
    public TextMeshProUGUI codeText;
    public TextMeshProUGUI errorField;

    public void Onclick()
    {
        codeText.text = Client.Instance.MdpDuLobby;
        if(codeText.text == ""){
            errorField.text = "Demandez au chez du lobby de partager le code!";
        }
    }
}
