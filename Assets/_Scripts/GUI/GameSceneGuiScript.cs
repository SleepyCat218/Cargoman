using UnityEngine;
using UnityEngine.UI;

namespace Cargoman
{
    public class GameSceneGuiScript : MonoBehaviour
    {
        [SerializeField] private Text _scoreValue, _timerValue;
        [SerializeField] private Image _timerImage;

        public void UpdateTimer(float timer, float startTimeValue)
        {
            _timerValue.text = ((int)timer).ToString();
            _timerImage.fillAmount = timer / startTimeValue;
        }

        public void UpdateScore(int score)
        {
            _scoreValue.text = score.ToString();
        }
    }
}