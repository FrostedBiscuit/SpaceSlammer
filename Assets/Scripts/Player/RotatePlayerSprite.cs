using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerSprite : MonoBehaviour {

    private void OnEnable() {

        transform.rotation = Quaternion.identity;
    }

    void Update() {
        
        if (Player.instance.GetRigidbody().velocity != Vector2.zero) {

            Vector2 vel = Player.instance.GetRigidbody().velocity;

            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg);
        }
    }
}
