using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    public float BaseOrthoSize = 0f;

    [SerializeField]
    private float NearZoom = 4f;
    [SerializeField]
    private float FarZoom = 6f;
    [SerializeField]
    private float ZoomSpeed = 5f;

    //float lastDmgMul;
    float currOrthoLerpAmt;

    void Update() {

        if (Player.instance.gameObject.activeSelf == true) {

            currOrthoLerpAmt = Mathf.Lerp(currOrthoLerpAmt, Player.instance.DamageMultiplier > 1f ? FarZoom : NearZoom, Time.deltaTime * ZoomSpeed);

            Camera.main.orthographicSize = BaseOrthoSize + currOrthoLerpAmt; //Mathf.Lerp(Camera.main.orthographicSize, Player.instance.DamageMultiplier > 1f ? FarZoom : NearZoom, Time.deltaTime * ZoomSpeed);
        }
        else {

            currOrthoLerpAmt = Mathf.Lerp(currOrthoLerpAmt, NearZoom, Time.deltaTime * ZoomSpeed);

            Camera.main.orthographicSize = BaseOrthoSize + currOrthoLerpAmt; //Mathf.Lerp(Camera.main.orthographicSize, NearZoom, Time.deltaTime * ZoomSpeed);
        }
    }
}
