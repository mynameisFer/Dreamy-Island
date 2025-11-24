using UnityEngine;
using UnityEngine.SceneManagement;   

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance { get; private set; }

    [Header("Player Life")]
    [SerializeField] int maxLife = 3;   
    public int CurrentLife { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentLife = maxLife;
    }

    public void LoseLife()
    {
        CurrentLife--;
        Debug.Log("LoseLife called. Life left: " + CurrentLife);

        if (CurrentLife <= 0)
        {
            GameOver();
        }
        else
        {
            ReloadCurrentScene();
        }
    }

    void ReloadCurrentScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    void GameOver()
    {
        Debug.Log("Game Over");
        ReloadCurrentScene();
    }

    public void ResetLife()
    {
        CurrentLife = maxLife;
    }
}

