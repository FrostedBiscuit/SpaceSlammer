using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyIndicatorPool : ObjectPool<UIEnemyIndicator> {

    #region Singelton
    public static UIEnemyIndicatorPool instance = null;

    private void Awake() {

        if (instance != null) {

            Debug.LogError($"{this.name}::Awake() => More than 1 instance of {this.name} in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField]
    Transform parentTransfrom = null;

    public override void Start() {

        PooledObject.gameObject.SetActive(false);

        for (int i = 0; i < NumToSpawn; i++) {

            UIEnemyIndicator ei = Instantiate(PooledObject, parentTransfrom);

            pooledObjectQueue.Enqueue(ei);
        }

        PooledObject.gameObject.SetActive(true);
    }

    [Obsolete("Don't use this function to request objects in this class", true)]
    public override UIEnemyIndicator RequestObject(Vector3 position, Quaternion rotation) {

        throw new Exception("Do not use RequestObject with parameters in this class");
    }

    public UIEnemyIndicator RequestObject() {

        UIEnemyIndicator ei = pooledObjectQueue.Dequeue();

        ei.gameObject.SetActive(true);

        return ei;
    }
}
