using UnityEngine;
using UnityEngine.UI;

public class GameSceneGuiScript : MonoBehaviour
{
    [SerializeField] private Text _scoreValue;

    void Start()
    {
        ScoreManager.Instance.SetGuiScript(this);
    }

    public void UpdateScore(int score)
    {
        _scoreValue.text = score.ToString();
    }
}
