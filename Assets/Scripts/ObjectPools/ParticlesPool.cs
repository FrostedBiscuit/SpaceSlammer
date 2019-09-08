using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPool : ObjectPool<Particles> {

    #region Singelton
    public static ParticlesPool instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("ParticlesPool::Awake() => More than 1 instance of ParticlesPool in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start() {

        for (int i = 0; i < NumToSpawn; i++) {

            Particles p = Instantiate(PooledObject, transform);

            p.gameObject.SetActive(false);

            pooledObjectQueue.Enqueue(p);
        }
    }
}
