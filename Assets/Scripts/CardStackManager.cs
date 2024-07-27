using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CardStackManager : MonoBehaviour
{
    [Header("Card Indicators")]
    public List<GameObject> Cards;
    public List<Sprite> pictureList;

    [Header("Game Info")]
    private string gameMode = "easy";
    private int totalCards;

    void Start()
    {
        totalCards = gameMode switch
        {
            "easy" => 16,
            "medium" => 24,
            "hard" => 32,
            _ => throw new System.NotImplementedException(),
        };
        GetCards();
    }

    void GetCards()
    {
        foreach (Transform cardTransform in GetComponentInChildren<Transform>())
        {
            Cards.Add(cardTransform.gameObject);
        }
    }

    void Update()
    {

    }

    void RandomizeCards()
    {

        // random card id
        // repeat id twice
    }

}
