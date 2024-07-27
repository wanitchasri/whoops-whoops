using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Shuffle(cardPictures);
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

    void Shuffle<T>(List<T> inputList)
    {
        for (int i = 0; i < inputList.Count; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }

    void SetCardPics()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            GameObject card = Cards[i];
            Image cardPic = card.transform.Find("Canvas/CardPicture").GetComponent<Image>();
            cardPic.sprite = cardPictures[i];
            card.GetComponent<CardManager>().cardIndicator = cardPic.sprite.name;
        }
    }



}
