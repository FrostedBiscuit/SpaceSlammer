using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

    public float ParallaxCoeficient = 3f;

    void Update() {

        Material mat = GetComponent<MeshRenderer>().material;

        Vector2 offset = new Vector2(transform.position.x / transform.localScale.x / ParallaxCoeficient, transform.position.y / transform.localScale.y / ParallaxCoeficient);

        mat.mainTextureOffset = offset;
    }
}
