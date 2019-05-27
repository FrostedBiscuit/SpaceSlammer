using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleSwitch : MonoBehaviour {

    [SerializeField]
    Animator Animator = null;

    [SerializeField]
    Toggle Toggle;

    // Start is called before the first frame update
    private void OnEnable() {
        
        if (Animator == null) {
            Debug.LogError("UIToggleSwitch::Start() => No Animator assigned!!!");
            return;
        }

        if (Toggle == null) {
            Debug.LogError("UIToggleSwitch::Start() => No Toggle assigned!!!");
            return;
        }

        //Toggle.onValueChanged.Invoke(Toggle.isOn);
    }

    public void ToggleSwitchAnimate(bool value) {

        if (Animator.gameObject.activeSelf) {
            Animator.SetBool("IsToggled", value);
        }
    }
}
