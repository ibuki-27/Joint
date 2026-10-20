using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameOverManager _gameOverManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _gameOverManager.GameClear();
        }
    }
}
