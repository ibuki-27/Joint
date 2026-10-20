using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private float _timeLimit = 60f;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private GameOverManager _gameOverManager;

    private float _remainingTime;
    private bool _isFinished;

    private void Start()
    {
        _remainingTime = _timeLimit;
        UpdateTimerText();
    }

    private void Update()
    {
        if (_isFinished) return;

        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0f)
        {
            _remainingTime = 0f;
            _isFinished = true;

            _gameOverManager.GameOver();
        }

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        _timerText.text = Mathf.CeilToInt(_remainingTime).ToString();
    }
}
