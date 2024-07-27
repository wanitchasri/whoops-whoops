using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CardStackManager : MonoBehaviour
{
    [Header("Card Indicators")]
    public List<GameObject> Cards;
    public List<Sprite> pictureList;

    [Header("Game Info")]
    private string gameMode = "easy";
    private int totalCards;
    private List<Sprite> cardPictures;

    void Start()
    {
        SetTotalCards();
        GetCards();
        PlaceCards();
        SetCardPics();
    }

    void SetTotalCards()
    {
        totalCards = gameMode switch
        {
            "easy" => 16,
            "medium" => 24,
            "hard" => 32,
            _ => throw new System.NotImplementedException(),
        };
    }

    void GetCards()
    {
        foreach (Transform cardTransform in GetComponentInChildren<Transform>())
        {
            Cards.Add(cardTransform.gameObject);
        }
    }

    void PlaceCards()
    {
        int totalPics = totalCards / 2;
        cardPictures = new List<Sprite>();
        for (int i = 0; i < totalPics; i++) { cardPictures.Add(pictureList[i]); }
        for (int i = 0; i < totalPics; i++) { cardPictures.Add(pictureList[i]); }
    }

    void ShuffleCards()
    {

    }

    void SetCardPics()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            GameObject card = Cards[i];
            Image cardPic = card.transform.Find("Canvas/CardPicture").GetComponent<Image>();
            cardPic.sprite = cardPictures[i];
        }
    }

    //void RandomizePictures()
    //{
    //    int totalPics = totalCards/2;
    //    cardPictures = new Sprite[totalPics];

        

    //    // random card id from range

    //    // repeat id twice
    //}

    void Update()
    {

    }

}
