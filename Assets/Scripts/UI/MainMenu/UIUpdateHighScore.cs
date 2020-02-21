using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpdateHighScore : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI HighScoreText = null;

    private void OnEnable() {

        HighScoreText.text = ScoreManager.instance.GetHighScore().ToString();
    }
}
