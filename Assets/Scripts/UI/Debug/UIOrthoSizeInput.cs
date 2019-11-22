using System;
using UnityEngine;
using TMPro;

public class UIOrthoSizeInput : MonoBehaviour {

    public TMP_InputField InputField;

    public CameraZoom CamZoom;

    private void OnEnable() {

        InputField.text = CamZoom.BaseOrthoSize.ToString();
    }

    public void SetOrthoSize(string value) {

        float lastOrthoSize = CamZoom.BaseOrthoSize;

        try {

            CamZoom.BaseOrthoSize = float.Parse(value);
        }
        catch (Exception e) {

            Debug.LogError(e);

            CamZoom.BaseOrthoSize = lastOrthoSize;
        }
    }
}
