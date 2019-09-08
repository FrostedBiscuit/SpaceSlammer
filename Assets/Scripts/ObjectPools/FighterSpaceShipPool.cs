using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSpaceShipPool : ObjectPool<Enemy> {

    #region Singelton
    public static FighterSpaceShipPool instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("FighterSpaceShipPool::Awake() => More than 1 instacne of FighterSpaceShipPool in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start() {

        for (int i = 0; i < NumToSpawn; i++) {

            Enemy fss = Instantiate(PooledObject, transform);

            fss.gameObject.SetActive(false);

            pooledObjectQueue.Enqueue(fss);
        }
    }
}
