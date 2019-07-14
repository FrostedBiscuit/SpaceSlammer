using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollapsablePanel : MonoBehaviour {

    bool isActive = false;

    [SerializeField]
    Animator Animator = null;

    public void Toggle() {

        isActive = !isActive;

        Animator.SetBool("isOpen", isActive);
    }
}
