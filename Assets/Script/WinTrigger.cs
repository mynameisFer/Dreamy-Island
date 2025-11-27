using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null )
        {
            Time.timeScale = 0f;
            UIManager.Instance?.ShowWin(true);
            
        }
    }
}
