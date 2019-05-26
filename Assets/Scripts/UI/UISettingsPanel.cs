using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingsPanel : MonoBehaviour {

    bool isActive = false;

    [SerializeField]
    Animator Animator;

    public void ToggleSettingsPanel() {

        isActive = !isActive;

        Animator.SetBool("isOpen", isActive);
    }
}
