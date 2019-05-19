using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdateHealthSlider : MonoBehaviour {

    Slider slider;

    // Start is called before the first frame update
    void Start() {

        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update() {
        
        if (Player.instance == null) {

            slider.value = 0f;
        }
        else {

            slider.value = Player.instance.Health / Player.instance.MaxHealth;
        }
    }
}
