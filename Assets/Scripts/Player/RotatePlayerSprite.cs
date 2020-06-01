using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerSprite : MonoBehaviour {

    [SerializeField]
    Rigidbody2D PlayerRB = null;

    private void OnEnable() {

        transform.rotation = Quaternion.identity;
    }

    void Update() {
        
        if (PlayerRB.velocity != Vector2.zero) {

            Vector2 vel = PlayerRB.velocity;

            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg);
        }
    }
}
