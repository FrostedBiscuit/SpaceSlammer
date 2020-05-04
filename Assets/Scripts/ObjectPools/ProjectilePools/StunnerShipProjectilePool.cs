using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnerShipProjectilePool : ObjectPool<StunnerShipProjectile> {

    #region Singelton
    public static StunnerShipProjectilePool instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("StunnerShipProjectilePool::Awake() => More than 1 instance of StunnerShipProjectilePool in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion
}
