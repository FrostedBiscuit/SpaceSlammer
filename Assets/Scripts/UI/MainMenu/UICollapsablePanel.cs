using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollapsablePanel : MonoBehaviour {

    bool isActive = false;

    [SerializeField]
    Animator Animator = null;

    private void OnEnable() {

        Animator.SetBool("isOpen", isActive);
    }

    public void Toggle() {

        isActive = !isActive;

        Animator.SetBool("isOpen", isActive);
    }
}
