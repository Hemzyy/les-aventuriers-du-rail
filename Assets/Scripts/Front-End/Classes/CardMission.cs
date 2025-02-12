using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardMission
{
    public string station1;
    public string station2;
    public int points;
    public int id;

    public CardMission(){}

    public CardMission(string s1, string s2, int pts)
    {
        this.station1 = s1;
        this.station2 = s2;
        this.points = pts;
    }
    public CardMission(int id, string s1, string s2, int pts)
    {
        this.id = id;
        this.station1 = s1;
        this.station2 = s2;
        this.points = pts;
    }
}
