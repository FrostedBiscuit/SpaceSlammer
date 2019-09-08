using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinePool : ObjectPool<Mine> {

    #region Singelton
    public static MinePool instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("MinePool::Awake() => More than 1 instance of MinePool in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start() {

        for (int i = 0; i < NumToSpawn; i++) {

            Mine m = Instantiate(PooledObject, transform);

            m.gameObject.SetActive(false);

            pooledObjectQueue.Enqueue(m);
        }
    }
}
