using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Score Texts")]
    public TMP_Text ScoreText;
    public TMP_Text MatchesText;
    public TMP_Text TurnsText;

    public void OnHomeButtonClicked()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
