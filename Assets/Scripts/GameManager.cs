using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Score Texts")]
    public TMP_Text MatchesText;
    public TMP_Text TurnsText;
    public TMP_Text ScoreText;

    [Header("Game Info")]
    private int matches;
    private int turns;
    private int score;
    private int clickedCardsQty;
    private List<GameObject> clickedCards;
    private float flipBackDelay = 1f;

    private void Start()
    {
        clickedCards = new List<GameObject>();    
    }

    private void Update()
    {
        TurnsText.text = turns.ToString();
        MatchesText.text = matches.ToString();
        ScoreText.text = score.ToString();

        DetectCardClicks();
    }

    private void DetectCardClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedCard = hit.collider.gameObject;
                if (clickedCard.tag == "Card"
                    && clickedCardsQty < 2)
                {
                    CardManager cardManager = clickedCard.GetComponent<CardManager>();
                    if (!cardManager.cardClicked)
                    {
                        StartCoroutine(cardManager.FlipCard(-180f));
                        clickedCardsQty++;
                        clickedCards.Add(clickedCard);

                        if (clickedCardsQty == 2) { StartCoroutine(VerifyCardMatch()); }
                    }

                }
            }
        }
    }

    IEnumerator VerifyCardMatch()
    {
        turns++;
        yield return new WaitForSeconds(flipBackDelay);

        CardManager firstCardManager = clickedCards[0].GetComponent<CardManager>();
        CardManager secondCardManager = clickedCards[1].GetComponent<CardManager>();

        if (firstCardManager.cardIndicator == secondCardManager.cardIndicator)
        {
            matches++;
            clickedCards[0].SetActive(false);
            clickedCards[1].SetActive(false);
        }
        else
        {
            StartCoroutine(firstCardManager.FlipCard(0f));
            StartCoroutine(secondCardManager.FlipCard(0f));
        }

        clickedCardsQty = 0;
        clickedCards.Clear();

        // temporary score
        score = matches;
    }

    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
