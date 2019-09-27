using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    [SerializeField]
    private float NearZoom = 4f;
    [SerializeField]
    private float FarZoom = 6f;
    [SerializeField]
    private float ZoomSpeed = 5f;

    float lastDmgMul;

    void Update() {

        if (Player.instance.gameObject.activeSelf == true) {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Player.instance.DamageMultiplier > 1f ? FarZoom : NearZoom, Time.deltaTime * ZoomSpeed);
        }
        else {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, NearZoom, Time.deltaTime * ZoomSpeed);
        }
    }
}
