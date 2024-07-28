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
    private int combo;

    [Header("Cards Info")]
    public GameObject[] CardLayoutOptions;
    private GameObject CardStack;
    private int clickedCardsQty;
    private List<GameObject> clickedCards;
    private float flipBackDelay = 1f;

    [Header("End Panel")]
    public GameObject EndGamePanel;
    public TMP_Text EndingScoreText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip cardFlipSFX;
    public AudioClip wrongSFX;
    public AudioClip correctSFX;
    public AudioClip winSFX;

    public delegate void GameActivityUpdated();
    public event GameActivityUpdated OnGameEndedActivity;

    private void Awake()
    {
        Instance = this;

        string gameMode = MenuManager.Instance.gameMode;
        int modeID = gameMode switch { "Easy" => 0, "Medium" => 1, _ => 2, };
        CardStack = CardLayoutOptions[modeID];
        CardStack.SetActive(true);
    }

    private void OnEnable()
    {
        OnGameEndedActivity += OnGameEnded;
    }

    private void OnDisable()
    {
        OnGameEndedActivity -= OnGameEnded;
    }

    public void LoadData(GameData data)
    {
        this.matches = data.matches;
        this.turns = data.turns;
        this.score = data.score;
        this.combo = data.combo;
    }

    public void SaveData(ref GameData data)
    {
        data.matches = this.matches;
        data.turns = this.turns;
        data.score = this.score;
        data.combo = this.combo;
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
                        audioSource.PlayOneShot(cardFlipSFX);
                        StartCoroutine(cardManager.FlipCard(-180f));

                        clickedCardsQty++;
                        clickedCards.Add(clickedCard);

                        List<int> cardFlipInfo = CardStack.GetComponent<CardStackManager>().cardFlipInfo;
                        cardFlipInfo[cardManager.cardIndex] = 1;

                        if (clickedCardsQty == 2) { StartCoroutine(VerifyCardMatch(cardFlipInfo)); }
                    }

                }
            }
        }
    }

    IEnumerator VerifyCardMatch(List<int> cardFlipInfo)
    {
        turns++;
        yield return new WaitForSeconds(flipBackDelay);

        CardManager firstCardManager = clickedCards[0].GetComponent<CardManager>();
        CardManager secondCardManager = clickedCards[1].GetComponent<CardManager>();

        int flipInfo = 0;
        if (firstCardManager.cardIndicator == secondCardManager.cardIndicator)
        {
            combo++;
            score += combo;
            matches++;

            clickedCards[0].SetActive(false);
            clickedCards[1].SetActive(false);
            audioSource.PlayOneShot(correctSFX);
            flipInfo = -1;
        }
        else
        {
            combo = 0;
            StartCoroutine(firstCardManager.FlipCard(0f));
            StartCoroutine(secondCardManager.FlipCard(0f));
            audioSource.PlayOneShot(wrongSFX);
        }

        cardFlipInfo[firstCardManager.cardIndex] = flipInfo;
        cardFlipInfo[secondCardManager.cardIndex] = flipInfo;

        clickedCardsQty = 0;
        clickedCards.Clear();

        if (matches == CardStackManager.Instance.totalCards / 2) { OnGameEndedActivity.Invoke(); }
    }

    public void OnGameEnded()
    {
        EndingScoreText.text = score.ToString();
        EndGamePanel.SetActive(true);
        audioSource.PlayOneShot(winSFX);
    }

    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
