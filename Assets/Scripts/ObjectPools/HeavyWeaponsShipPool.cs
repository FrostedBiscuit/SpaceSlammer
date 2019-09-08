using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeaponsShipPool : ObjectPool<Enemy> {

    #region Singelton
    public static HeavyWeaponsShipPool instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("HeavyWeaponsShipPool::Awake() => More than 1 instance of HeavyWeaponsShipPool in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start() {

        for (int i = 0; i < NumToSpawn; i++) {

            Enemy hws = Instantiate(PooledObject, transform);

            hws.gameObject.SetActive(false);

            pooledObjectQueue.Enqueue(hws);
        }
    }
}
