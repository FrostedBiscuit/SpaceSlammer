using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAccelerationInput : MonoBehaviour {

    public TMP_InputField InputField;

    public SwipeInput SwipeInput;

    private void OnEnable() {

        InputField.text = SwipeInput.AccelerationMultiplier.ToString();
    }
}
