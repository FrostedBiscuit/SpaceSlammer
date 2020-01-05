using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInvincibilityIcon : UIConsumableIcon {

    #region Singelton
    public static UIInvincibilityIcon instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("UIInvincibilityIcon::Awake() => More than 1 instance in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion
}
