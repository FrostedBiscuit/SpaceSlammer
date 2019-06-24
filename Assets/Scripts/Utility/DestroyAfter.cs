using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {

    public float Delay = 2f;

    private void OnEnable() {

        StartCoroutine(destroyAfter());
    }

    IEnumerator destroyAfter() {

        yield return new WaitForSeconds(Delay);

        ObjectPool.instance.ReturnObject(gameObject);
    }
}
