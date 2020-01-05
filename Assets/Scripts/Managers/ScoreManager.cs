using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {

    #region Singelton
    public static ScoreManager instance;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("ScoreManager::Awake() => More than 1 instance of ScoreManager in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion

    public TextMeshProUGUI score;

    int currScore;
    int highScore;

    private void Start() {

        if (PlayerPrefs.HasKey("HighScore")) {

            highScore = PlayerPrefs.GetInt("HighScore");
        }
        else {

            PlayerPrefs.SetInt("HighScore", 0);

            highScore = 0;
        }
     }

    void Update() {

        if (Player.instance.gameObject.activeSelf == false) {

            PlayerPrefs.Save();

            return;
        }

        score.text = currScore.ToString();
    }

    public int GetHighScore() {

        return highScore;
    }

    public int GetCurrentScore() {

        return currScore;
    }

    public void ClearCurrentScore() {

        currScore = 0;
    }

    public void CalculateScore(float time) {

        currScore += Mathf.RoundToInt(time * 0.5f);

        Debug.Log($"Current score: {currScore}");
    }

    public void UpdateCurrentScore(int amt) {

        currScore += amt;

        if (highScore < currScore) {

            highScore = currScore;

            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
}
