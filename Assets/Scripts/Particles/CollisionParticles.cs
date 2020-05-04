using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticles : MonoBehaviour, IDisposable {

    public float DestoryAfter = 2f;

    private void OnEnable() {

        StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter() {

        yield return new WaitForSeconds(DestoryAfter);

        CollisionParticlesPool.instance.ReturnObject(this);
    }

    public void Dispose() {

        StopAllCoroutines();

        CollisionParticlesPool.instance.ReturnObject(this);
    }
}
