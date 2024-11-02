using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    [Header("Saved Variables")]
    public int currency;
    public Vector2 position;

    //constructor, ran on starting new game
    public GameData()
    {
        this.currency = 0;
        this.position = new Vector2(0, -1f);
    }
}
