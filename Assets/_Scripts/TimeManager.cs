using System.Collections;
using UnityEngine;

namespace Cargoman
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private GameSceneGuiScript _gui;

        [SerializeField] private float _startTimerValue = 600f; 

        private void Awake()
        {
            StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer()
        {
            float timer = _startTimerValue, oldTimer = _startTimerValue;

            while (true)
            {
                timer = _startTimerValue - Time.time;

                if ((int)timer != (int)oldTimer)
                {
                    _gui.UpdateTimer(timer, _startTimerValue);
                }
                oldTimer = timer;
                yield return new WaitForSeconds(.5f);
            }
        }
    }
}