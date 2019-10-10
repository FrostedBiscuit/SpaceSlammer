using System;
using UnityEngine;
using TMPro;

public class UIOrthoSizeInput : MonoBehaviour {

    public TMP_InputField InputField;

    public Camera Cam;

    private void OnEnable() {

        InputField.text = Cam.orthographicSize.ToString();
    }

    public void SetOrthoSize(string value) {

        float lastOrthoSize = Cam.orthographicSize;

        try {

            Cam.orthographicSize = float.Parse(value);
        }
        catch (Exception e) {

            Debug.LogError(e);

            Cam.orthographicSize = lastOrthoSize;
        }
    }
}
