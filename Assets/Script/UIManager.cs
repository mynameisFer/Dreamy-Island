using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Coin UI")]
    public TMP_Text coinText;

    [Header("Heart UI")]
    public Transform heartContainer;
    public GameObject heartPrefab;
    public int maxHearts = 3;

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;

    int currentCoins = 0;
    int currentLife = 0;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        currentCoins = 0;
        int gmMax = maxHearts;
        int gmCurrent = maxHearts;

        if (GameManager.instance != null)
        {
           
            gmCurrent = GameManager.instance.CurrentLife;
           
        }

        SetupHearts(gmMax);       
        UpdateHearts(gmCurrent);  
        UpdateCoinText();
    }


    public void AddCoin(int amount)
    {
        currentCoins += amount;
        UpdateCoinText();
    }
    void UpdateCoinText()
    {
        if (coinText != null)
            coinText.text = currentCoins.ToString();
    }
   
   
    public void SetupHearts(int count)
    {
        foreach (Transform t in heartContainer) Destroy(t.gameObject);

        maxHearts = count;
        for (int i = 0; i < maxHearts; i++)
        {
            Instantiate(heartPrefab, heartContainer);
        }

    }

    public void UpdateHearts(int life)
    {
        life = Mathf.Clamp(life, 0, heartContainer.childCount);

        for (int i = 0; i < heartContainer.childCount; i++)
        {
            heartContainer.GetChild(i).gameObject.SetActive(i < life);
        }
    }

    public void OnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }

    public void ShowPause(bool show)
    {
        {
            if (pausePanel != null) pausePanel.SetActive(show);
            Time.timeScale = show ? 0f : 1f;
        }
    }
    public void ShowWin(bool show)
    {
        {
            if (winPanel != null) winPanel.SetActive(show);
        }
    }
    public void ShowLose(bool show) => losePanel?.SetActive(show);
}
