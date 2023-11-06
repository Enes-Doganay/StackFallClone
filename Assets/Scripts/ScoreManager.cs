using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : Singleton<ScoreManager>
{
    public int score;
    public Text scoreText;

    protected override void Awake()
    {
        base.Awake();

        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        }
    }
    private void Start()
    {
        AddScore(0);
    }
    /*
    private void Update()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        }
    }
    */
    public void AddScore(int value)
    {
        score += value;
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            Debug.Log("HighScore" + PlayerPrefs.GetInt("HighScore"));
        }
        scoreText.text = score.ToString();
    }
    public void ResetScore()
    {
        score = 0;
    }
}
