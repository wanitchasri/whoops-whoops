using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int score;
    public int matches;
    public int turns;
    public int combo;
    public List<Sprite> cardPictures;
    public List<int> cardFlipInfo;
    public bool isNewGame;

    // default values
    public GameData()
    {
        this.score = 0;
        this.matches = 0;
        this.turns = 0;
        this.combo = 0;
        this.cardPictures = new List<Sprite>();
        this.cardFlipInfo = new List<int>();
        this.isNewGame = true;
    }
}
