using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Class pour représenter les cartes trains côté front-end
public class CardTrain
{
    public Color32 color;
    public string symbole;
    public int count;

    public CardTrain(){}

    public CardTrain(Color32 Color, string Symbole)
    {
        this.color = Color;
        this.symbole = Symbole;
    }

    void Start()
    {
        
    }
}
