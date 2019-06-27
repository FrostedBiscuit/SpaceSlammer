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
        
        score.text = EnemyManager.instance.sumEnemyDied.ToString();

        if (highScore < EnemyManager.instance.sumEnemyDied) {

            PlayerPrefs.SetInt("HighScore", EnemyManager.instance.sumEnemyDied);

            highScore = EnemyManager.instance.sumEnemyDied;
        }
    }

    public int GetHighScore() {

        return highScore;
    }
}
