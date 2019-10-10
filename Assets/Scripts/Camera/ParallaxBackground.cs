using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

    public float ParallaxCoeficient = 3f;

    public Vector2 ConstScrollWeights = new Vector2();

    void Update() {

        Material mat = GetComponent<MeshRenderer>().material;

        Vector2 offset = new Vector2((transform.position.x + ConstScrollWeights.x * Time.time) / transform.localScale.x / ParallaxCoeficient, (transform.position.y + ConstScrollWeights.y * Time.time) / transform.localScale.y / ParallaxCoeficient);

        mat.mainTextureOffset = offset;
    }
}
