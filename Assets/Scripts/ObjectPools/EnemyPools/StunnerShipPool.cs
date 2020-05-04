using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnerShipPool : ObjectPool<Enemy> {

    #region Singelton
    public static StunnerShipPool instance = null;

    private void Awake() {

        if (instance != null) {

            Debug.LogError("StunnerShipPool::Awake() => More than 1 instance of StunnerShipPool in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion
}
