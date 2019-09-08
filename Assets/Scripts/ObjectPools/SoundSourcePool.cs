using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSourcePool : ObjectPool<SoundSource> {

    #region Singelton
    public static SoundSourcePool instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("SoundSourcePool::Awake() => More than 1 instance of SoundSourcePool in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start() {

        for (int i = 0; i < NumToSpawn; i++) {

            SoundSource obj = Instantiate(PooledObject, transform);

            obj.gameObject.SetActive(false);

            pooledObjectQueue.Enqueue(obj);
        }
    }
}
