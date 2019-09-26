using System.Collections;
using UnityEngine;

namespace Cargoman
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private GameSceneGuiScript _gui;
        [SerializeField] private float _startTimerValue = 600f;

        private float _currentSceneTimer;
        private Coroutine _timerCoroutine;

        private void Awake()
        {
            _currentSceneTimer = Time.time;
            _timerCoroutine = StartCoroutine(StartTimer());
        }

        private void EndGame()
        {
            StopCoroutine(_timerCoroutine);
            _gui.EndGame();
            Time.timeScale = 0;
        }

        private IEnumerator StartTimer()
        {
            float timer = _startTimerValue, oldTimer = _startTimerValue;

            while (timer > 0)
            {
                timer = _startTimerValue - Time.time + _currentSceneTimer;

                if ((int)timer != (int)oldTimer)
                {
                    _gui.UpdateTimer(timer, _startTimerValue);
                }
                oldTimer = timer;
                yield return new WaitForSeconds(.5f);
            }

            _gui.UpdateTimer(0, _startTimerValue);
            EndGame();
        }
    }
}