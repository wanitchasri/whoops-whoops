using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance;

    [Header("Score Texts")]
    public TMP_Text MatchesText;
    public TMP_Text TurnsText;
    public TMP_Text ScoreText;

    [Header("Score Info")]
    private int matches;
    private int turns;
    private int score;

    [Header("Cards Info")]
    private int clickedCardsQty;
    private List<GameObject> clickedCards;
    private float flipBackDelay = 1f;

    public delegate void GameActivityUpdated();
    public event GameActivityUpdated OnGameEndedActivity;
    public GameObject EndGamePanel;
    public TMP_Text EndingScoreText;

    public List<int> cardFlipInfo;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        OnGameEndedActivity += OnGameEnded;
    }

    public void LoadData(GameData data)
    {
        this.matches = data.matches;
        this.turns = data.turns;
        this.score = data.score;
    }

    public void SaveData(ref GameData data)
    {
        data.matches = this.matches;
        data.turns = this.turns;
        data.score = this.score;
    }

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
        if (matches == CardStackManager.Instance.totalCards / 2) { OnGameEndedActivity.Invoke(); }
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
                        cardFlipInfo = CardStackManager.Instance.cardFlipInfo;
                        cardFlipInfo[cardManager.cardIndex] = 1;

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

        int flipInfo = 0;
        if (firstCardManager.cardIndicator == secondCardManager.cardIndicator)
        {
            matches++;
            clickedCards[0].SetActive(false);
            clickedCards[1].SetActive(false);

            flipInfo = -1;
        }
        else
        {
            StartCoroutine(firstCardManager.FlipCard(0f));
            StartCoroutine(secondCardManager.FlipCard(0f));
        }

        cardFlipInfo[firstCardManager.cardIndex] = flipInfo;
        cardFlipInfo[secondCardManager.cardIndex] = flipInfo;

        clickedCardsQty = 0;
        clickedCards.Clear();

        // temporary score
        score = matches;
    }

    public void OnGameEnded()
    {
        EndingScoreText.text = score.ToString();
        EndGamePanel.SetActive(true);
    }

    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
