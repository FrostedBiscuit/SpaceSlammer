using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RadiusVisualizer : MonoBehaviour {

    [SerializeField]
    new ParticleSystem particleSystem = null;

    public void SetRange(float r) {

        ParticleSystem.MainModule main = particleSystem.main;

        main.startSize = Mathf.Abs(r) * 4f;

        //Range = Mathf.Abs(r);
    }

    public void SetColor(Color color) {

        ParticleSystem.MainModule main = particleSystem.main;

        main.startColor = color;

        //lineRenderer.startColor = color;
        //lineRenderer.endColor = color;
    }

    private void Start() {

        if (particleSystem == null) {

            Debug.LogError("RadiusVisualizer::Start() => Particle system not found!!!");

            return;
        }

        //generateNormalizedVerticies();
    }

    private void Update() {
#if UNITY_EDITOR
        //generateNormalizedVerticies();
#endif
        /*Vector3[] newVerticies = new Vector3[normalizedVerticies.Length];

        for (int i = 0; i < newVerticies.Length; i++) {
            newVerticies[i] = normalizedVerticies[i] * Range + transform.position;
        }

        lineRenderer.SetPositions(newVerticies);*/
    }
    /*
    private void generateNormalizedVerticies() {

        if (normalizedVerticies != null && normalizedVerticies.Length + 1 == NumVerticies) {
            return;
        }

        normalizedVerticies = new Vector3[NumVerticies];

        for (int i = 0; i < NumVerticies; i++) {

            float angle = ((float)(i) / (float)(NumVerticies - 1)) * Mathf.PI * 2f;

            normalizedVerticies[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        normalizedVerticies[NumVerticies - 1] = normalizedVerticies[0];

        lineRenderer.positionCount = NumVerticies;
    }*/
}
