using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score = 0;
    private GameSceneGuiScript _gui;
    private static ScoreManager _instance;

    public int Score
    {
        get
        {
            return _score;
        }
    }

    public static ScoreManager Instance
    {
        get
        {
            return _instance;
        }
        private set { }
    }

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
        DontDestroyOnLoad(gameObject);
    }

    public void SetGuiScript(GameSceneGuiScript gui)
    {
        _gui = gui;
    }

    public void AddScore(int scoreValue)
    {
        _score += scoreValue;
        _gui.UpdateScore(_score);
    }
}
