using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitronPool : ObjectPool<Enemy> {

    #region Singelton
    public static GravitronPool instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("GravitronPool::Awake() => More than 1 instance of GravitronPool in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion
}
