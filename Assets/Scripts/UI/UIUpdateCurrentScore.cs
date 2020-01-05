using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIUpdateCurrentScore : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI Label = null;

    private void OnEnable() {

        Label.text = ScoreManager.instance.GetCurrentScore().ToString();
    }
}
