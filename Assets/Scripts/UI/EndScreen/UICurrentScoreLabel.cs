using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICurrentScoreLabel : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI Label = null;

    private void OnEnable() 
    {
        Label.text = $"SCORE: {ScoreManager.instance.GetCurrentScore().ToString()}";
    }
}
