using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager instance {  get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoseLife()
    {
        Debug.Log("Player lost 1 life.");
    }
}
