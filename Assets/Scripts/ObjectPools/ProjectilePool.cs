using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : ObjectPool<Projectile> {

    #region Singelton
    public static ProjectilePool instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("ProjectilePool::Awake() => More than 1 instance of ProjectilePool in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start() {

        for (int i = 0; i < NumToSpawn; i++) {

            Projectile projectile = Instantiate(PooledObject, transform);

            projectile.gameObject.SetActive(false);

            pooledObjectQueue.Enqueue(projectile);
        }
    }
}
