using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticles : MonoBehaviour {

    public float DestoryAfter = 2f;

    private void OnEnable() {

        StartCoroutine(destroyAfter());
    }

    IEnumerator destroyAfter() {

        yield return new WaitForSeconds(DestoryAfter);

        ExplosionParticlesPool.instance.ReturnObject(this);
    }
}
