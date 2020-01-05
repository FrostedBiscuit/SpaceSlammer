using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdateHealthSlider : MonoBehaviour {

    Image slider;

    // Start is called before the first frame update
    void Start() {

        slider = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        
        if (Player.instance == null) {

            slider.fillAmount = 0f;
        }
        else {

            slider.fillAmount = Player.instance.Health / Player.instance.MaxHealth;
        }
    }
}
