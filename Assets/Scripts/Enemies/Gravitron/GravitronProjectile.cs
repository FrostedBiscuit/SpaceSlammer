using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitronProjectile : MonoBehaviour {

    [SerializeField]
    float G = 6.67408f;
    [SerializeField]
    float DestroyAfter = 5f;
    [SerializeField]
    float BoostForce = 10f;
    [SerializeField]
    float Mass = 20f;

    [SerializeField]
    Rigidbody2D Rigidbody = null;

    [HideInInspector]
    public float Damage;

    private void OnEnable() {

        Rigidbody.AddForce(transform.up * BoostForce);

        Invoke("Destroy", DestroyAfter);
    }

    private void FixedUpdate() {

        if (Player.instance.gameObject.activeSelf == false) {
            return;
        }

        Rigidbody2D playerRB = Player.instance.GetRigidbody();

        Vector2 dir = transform.position - Player.instance.transform.position;

        float dist = dir.magnitude;
        // Force is calculated with Newton's formula for gravity
        float forceMagnitude = G * (Mass * playerRB.mass) / (dist * dist);

        playerRB.AddForce(dir.normalized * forceMagnitude * Time.fixedDeltaTime);
        Rigidbody.AddForce(-dir.normalized * forceMagnitude * Time.fixedDeltaTime);
    }

    private void Destroy() {

        GravitronProjectilePool.instance.ReturnObject(this);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.transform.tag == "Player") {

            Player.instance.TakeDamage(Damage);
        }
    }
}
