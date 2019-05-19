using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPlayer : MonoBehaviour {

    // Update is called once per frame
    void Update() {

        transform.position = Camera.main.WorldToScreenPoint(Player.instance.transform.position);
    }
}
