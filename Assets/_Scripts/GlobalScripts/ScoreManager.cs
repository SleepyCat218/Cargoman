using UnityEngine;

namespace Cargoman
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private GameSceneGuiScript _gui;

        private int _score = 0;
        private static ScoreManager _instance;

        public static ScoreManager Instance
        {
            get
            {
                return _instance;
            }
            private set { }
        }

        public int Score { get => _score; }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            //DontDestroyOnLoad(gameObject);
        }

        public void AddScore(int scoreValue)
        {
            _score += scoreValue;
            _gui.UpdateScore(_score);
        }
    }
}