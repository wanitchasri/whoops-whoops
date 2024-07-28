using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardStackManager : MonoBehaviour, IDataPersistence
{
    public static CardStackManager Instance;

    [Header("Card Indicators")]
    public List<GameObject> Cards;
    public List<Sprite> pictureList;

    [Header("Game Info")]
    public int totalCards;
    public List<Sprite> cardPictures;
    public List<int> cardFlipInfo;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameRestartedActivity += OnGameRestarted;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameRestartedActivity -= OnGameRestarted;   
    }

    public void LoadData(GameData data)
    {
        this.cardPictures = data.cardPictures;
        this.cardFlipInfo = data.cardFlipInfo;

        ArrangeCardStack();
    }

    void LoadCardFlipInfo()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            GameObject card = Cards[i];
            CardManager cardManager = card.GetComponent<CardManager>();

            int flipInfo = cardFlipInfo[cardManager.cardIndex];
            switch (flipInfo)
            {
                case 1:
                    card.transform.Rotate(0, -180, 0);
                    break;

                case -1:
                    card.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.cardPictures = this.cardPictures;
        data.cardFlipInfo = this.cardFlipInfo;
    }

    private void OnGameRestarted()
    {
        // ArrangeCardStack();
    }

    void ArrangeCardStack()
    {
        SetTotalCards();
        GetCards();
        PlaceCards();
        Shuffle(cardPictures);
        SetCardPics();
    }

    void SetTotalCards()
    {
        totalCards = MenuManager.Instance.gameMode switch
        {
            "Easy" => 16,
            "Medium" => 24,
            "Hard" => 32,
            _ => throw new System.NotImplementedException(),
        };
        for (int i = 0; i < totalCards; i++) { cardFlipInfo.Add(0); }
    }

    void GetCards()
    {
        if (Cards.Count > 0) { Cards.Clear(); }
        foreach (Transform cardTransform in GetComponentInChildren<Transform>())
        {
            GameObject Card = cardTransform.gameObject;
            if (!Card.activeSelf) 
            { 
                Card.SetActive(true);
                Card.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            Cards.Add(Card);
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

            CardManager cardManager = card.GetComponent<CardManager>();
            cardManager.cardIndex = i;
            cardManager.cardIndicator = cardPic.sprite.name;
        }

        LoadCardFlipInfo();
    }

}
