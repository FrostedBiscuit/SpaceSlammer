using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeShipPool : ObjectPool<Enemy> {

    #region Singelton
    public static KamikazeShipPool instance = null;

    private void Awake() {

        if (instance != null) {

            Debug.LogError("KamikazeShipPool::Awake() => More than 1 instance of KamikazeShipPool in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start() {

        for (int i = 0; i < NumToSpawn; i++) {

            Enemy ks = Instantiate(PooledObject, transform);

            ks.gameObject.SetActive(false);

            pooledObjectQueue.Enqueue(ks);
        }
    }
}
