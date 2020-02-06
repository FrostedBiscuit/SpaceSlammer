using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    public float DefaultZoom = 4f;
    public float ZoomExtension = 6f;

    [SerializeField]
    private float ZoomSpeed = 5f;

    //float lastDmgMul;
    float currOrthoLerpAmt;

    private void Start() {

        currOrthoLerpAmt = Camera.main.orthographicSize;
    }

    void Update() {

        if (Player.instance.gameObject.activeSelf == true) {

            currOrthoLerpAmt = Mathf.Lerp(currOrthoLerpAmt, Player.instance.DamageMultiplier > 1f ? DefaultZoom + ZoomExtension : DefaultZoom, Time.deltaTime * ZoomSpeed);

            Camera.main.orthographicSize = currOrthoLerpAmt; //Mathf.Lerp(Camera.main.orthographicSize, Player.instance.DamageMultiplier > 1f ? FarZoom : NearZoom, Time.deltaTime * ZoomSpeed);
        }
        else {

            currOrthoLerpAmt = Mathf.Lerp(currOrthoLerpAmt, DefaultZoom, Time.deltaTime * ZoomSpeed);

            Camera.main.orthographicSize = currOrthoLerpAmt; //Mathf.Lerp(Camera.main.orthographicSize, NearZoom, Time.deltaTime * ZoomSpeed);
        }
    }

    public void SetDefaultZoom(float zoom) {

        DefaultZoom = zoom;
    }
}
