using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RadiusVisualizer : MonoBehaviour {

    [SerializeField]
    int NumVerticies = 10;

    [SerializeField]
    float Range = 2.5f;

    [SerializeField]
    LineRenderer lineRenderer;

    Vector3[] normalizedVerticies;
    //
    public void SetRange(float r) {
        Range = Mathf.Abs(r);
    }

    public void SetColor(Color color) {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    private void Start() {

        if (lineRenderer == null) {

            Debug.LogError("RadiusVisualizer::Start() => Line renderer not found!!!");

            return;
        }

        generateNormalizedVerticies();
    }

    private void Update() {
#if UNITY_EDITOR
        //generateNormalizedVerticies();
#endif
        Vector3[] newVerticies = new Vector3[normalizedVerticies.Length];

        for (int i = 0; i < newVerticies.Length; i++) {
            newVerticies[i] = normalizedVerticies[i] * Range + transform.position;
        }

        lineRenderer.SetPositions(newVerticies);
    }

    private void generateNormalizedVerticies() {

        normalizedVerticies = new Vector3[NumVerticies];

        for (int i = 0; i < NumVerticies - 1; i++) {

            float angle = ((float)(i + 1) / (float)NumVerticies) * Mathf.PI * 2f;

            normalizedVerticies[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        normalizedVerticies[NumVerticies - 1] = normalizedVerticies[0];

        lineRenderer.positionCount = NumVerticies;

        Debug.Log("Drew circle");
    }
}
