using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHighScoreLabel : MonoBehaviour {

    [SerializeField]
    string Prefix = "";
    [SerializeField]
    string Suffix = "";

    [SerializeField]
    bool UpdateLabel = false;

    [SerializeField]
    TextMeshProUGUI Label = null;

    private void OnEnable()
    {
        Label.text = Prefix + ScoreManager.instance.GetHighScore().ToString() + Suffix;
    }

    private void Update()
    {
        if (UpdateLabel)
        {
            Label.text = Prefix + ScoreManager.instance.GetHighScore().ToString() + Suffix;
        }
    }
}
