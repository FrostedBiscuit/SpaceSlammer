using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDamageBoostIcon : UIConsumableIcon {

    #region Singelton
    public static UIDamageBoostIcon instance = null;

    private void Awake() {

        if (instance != null) {

            Debug.LogError("UIDamageBoostIcon::Awake() => More than 1 instance in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion
}
