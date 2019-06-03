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

    void Update() {

        if (Player.instance.gameObject.activeSelf == false) {

            PlayerPrefs.Save();

            return;
        }
        
        score.text = EnemyManager.instance.sumEnemyDied.ToString();

        PlayerPrefs.SetInt("HighScore", EnemyManager.instance.sumEnemyDied);
    }

    public int GetHighScore() {

        if (PlayerPrefs.HasKey("HighScore"))
            return PlayerPrefs.GetInt("HighScore");

        return 0;
    }
}
