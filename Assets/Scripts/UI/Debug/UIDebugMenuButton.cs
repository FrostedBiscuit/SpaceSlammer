using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuButton : MonoBehaviour {

    public GameObject DebugMenu;

    public void ToggleMenu() {

        DebugMenu.SetActive(!DebugMenu.activeSelf);
    }
}
