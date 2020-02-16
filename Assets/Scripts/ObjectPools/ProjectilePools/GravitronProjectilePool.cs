using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitronProjectilePool : ObjectPool<GravitronProjectile> {

    #region Singelton
    public static GravitronProjectilePool instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("GravitronProjectilePool::Awake() => More than 1 instance of GravitronProjectilePool in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion
}
