using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _gameClearPanel;

    private bool _isGameFinished = false;
    private void Start()
    {
        Time.timeScale = 1f;

        _gameOverPanel.SetActive(false);
        _gameClearPanel.SetActive(false);
    }

    private void Update()
    {
        if (!_isGameFinished) return;

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void GameOver()
    {
        if (_isGameFinished) return;

        _isGameFinished = true;
        _gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void GameClear()
    {
        if (_isGameFinished) return;

        _isGameFinished = true;
        _gameClearPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}
