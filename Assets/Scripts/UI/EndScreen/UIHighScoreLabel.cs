using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHighScoreLabel : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI Label = null;

    private void OnEnable()
    {
        Label.text = $"HIGHSCORE: {ScoreManager.instance.GetHighScore().ToString()}";
    }
}
