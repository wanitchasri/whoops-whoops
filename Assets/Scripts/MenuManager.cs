using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    public static bool enteredGame;

    public static MenuManager Instance;
    public string gameMode;
    public ToggleGroup ModeOptionsToggle;

    private void Awake()
    {
        Instance = this;    
    }

    public void OnPlayButtonClicked()
    {
        string chosenToggle = ModeOptionsToggle.ActiveToggles().First().gameObject.name;
        gameMode = chosenToggle.Replace("Toggle", "");
        SceneManager.LoadScene("GameScene");
        DontDestroyOnLoad(this);
    }
}
