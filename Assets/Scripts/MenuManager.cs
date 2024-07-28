using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public string gameMode;
    public ToggleGroup ModeOptionsToggle;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;    
    }

    public void OnPlayButtonClicked()
    {
        string chosenToggle = ModeOptionsToggle.ActiveToggles().First().gameObject.name;
        gameMode = chosenToggle.Replace("Toggle", "");
        Debug.Log(gameMode);
        SceneManager.LoadScene("GameScene");
    }

}
