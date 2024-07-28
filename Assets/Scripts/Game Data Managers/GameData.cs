using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int score;
    public int matches;
    public int turns;
    public List<Sprite> cardPictures;
    public List<int> cardMatchInfo;

    // default values
    public GameData()
    {
        this.score = 0;
        this.matches = 0;
        this.turns = 0;
        this.cardPictures = new List<Sprite>();
        this.cardMatchInfo = new List<int>();
    }
}
