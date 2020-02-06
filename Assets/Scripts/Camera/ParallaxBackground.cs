using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

    public float ParallaxCoeficient = 3f;

    public Vector2 ConstScrollWeights = new Vector2();

    MeshRenderer meshRenderer;

    private void Start() {

        meshRenderer = GetComponent<MeshRenderer>();

        // Scale the plane to fit the screen
        transform.localScale = new Vector3(Camera.main.orthographicSize * 5f * Screen.width / Screen.height, 
                                           Camera.main.orthographicSize * 5f * Screen.width / Screen.height);
    }

    void Update() {

        // Move the background image
        Vector2 offset = new Vector2((transform.position.x + ConstScrollWeights.x * Time.time) / transform.localScale.x / ParallaxCoeficient, 
                                     (transform.position.y + ConstScrollWeights.y * Time.time) / transform.localScale.y / ParallaxCoeficient);

        meshRenderer.material.mainTextureOffset = offset;
    }
}
