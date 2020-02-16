using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoosterPool : ObjectPool<DamageBooster> {

    #region Singelton
    public static DamageBoosterPool instance = null;

    private void Awake() {

        if (instance != null) {

            Debug.LogError($"{this.name}::Awake() => More than 1 instance of {this.name} in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion
}
