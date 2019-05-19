using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpdateHighScore : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI HighScoreText;

    private void OnEnable() {

        Debug.Log(ScoreManager.instance == null);

        HighScoreText.text = ScoreManager.instance.GetHighScore().ToString();
    }
}
