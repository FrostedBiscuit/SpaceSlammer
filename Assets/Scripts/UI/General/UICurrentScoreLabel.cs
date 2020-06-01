using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICurrentScoreLabel : MonoBehaviour {

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
        Label.text = Prefix + ScoreManager.instance.GetCurrentScore().ToString() + Suffix;
    }

    private void Update()
    {
        if (UpdateLabel)
        {
            Label.text = Prefix + ScoreManager.instance.GetCurrentScore().ToString() + Suffix;
        }
    }
}
