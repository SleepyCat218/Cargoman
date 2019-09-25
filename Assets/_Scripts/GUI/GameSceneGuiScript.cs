using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Cargoman
{
    public class GameSceneGuiScript : MonoBehaviour
    {
        [SerializeField] private Text _scoreValue, _timerValue, _endGameScoreValue;
        [SerializeField] private Transform _endGameMenu; 
        [SerializeField] private Image _timerImage;
        [SerializeField] private Button _restartButton, _exitButton;

        private void Awake()
        {
            _restartButton.onClick.AddListener(RestartGame);
            _exitButton.onClick.AddListener(ExitGame);
        }

        public void UpdateTimer(float timer, float startTimeValue)
        {
            _timerValue.text = ((int)timer).ToString();
            _timerImage.fillAmount = timer / startTimeValue;
        }

        public void UpdateScore(int score)
        {
            _scoreValue.text = score.ToString();
        }

        public void EndGame()
        {
            _endGameScoreValue.text = ScoreManager.Instance.Score.ToString();
            _endGameMenu.gameObject.SetActive(true);
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }
    }
}