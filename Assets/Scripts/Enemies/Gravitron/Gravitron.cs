using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitron : Enemy {

    public float WanderingSpeed = 50f;
    public float SlowDownRate = 5f;
    public float TargetTime = 2f;
    public float SlopeCoefficient = 2f;
    public float BackoffRange = 2f;

    public int TimesToFire = 3;

    public Transform Gun = null;

    bool isTargeting = false;

    public override void Dispose() {

        GravitronPool.instance.ReturnObject(this);
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        move();
    }

    protected override void Attack() {
        base.Attack();

        GravitronProjectilePool.instance.RequestObject(Gun.position, Gun.rotation).Damage = AttackDamage;
    }

    protected override void Update() {

        if (isTargeting == false && 
            distanceToPlayer < AttackRange && 
            Player.instance.gameObject.activeSelf == true) {

            StartCoroutine(TargetAndAttack());
        }
    }
    
    protected override void Die() {
        base.Die();

        GravitronPool.instance.ReturnObject(this);
    }

    private void move() { 

        if (isTargeting == false && Player.instance.gameObject.activeSelf == true) {

            // move normally
            float speed = SlopeCoefficient * distanceToPlayer - SlopeCoefficient * BackoffRange;
            
            rigidbody.AddForce(transform.up * speed * Time.fixedDeltaTime);
        }
        else if (isTargeting == true && Player.instance.gameObject.activeSelf == true) {
            
            // slow down
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, 
                                              Vector2.zero,
                                              SlowDownRate * Time.fixedDeltaTime);
        }
        else if (Player.instance.gameObject.activeSelf == false) {

            rigidbody.AddForce(transform.up * WanderingSpeed * Time.fixedDeltaTime);
        }
    }

    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, BackoffRange);
    }

    IEnumerator TargetAndAttack() {

        if (isTargeting) {

            yield return null;
        }

        isTargeting = true;

        Debug.Log("Targeting");

        for (int i = 0; i < TimesToFire; i++) {

            Attack();

            yield return new WaitForSeconds(TargetTime / TimesToFire);
        }

        isTargeting = false;

        Debug.Log("Not targeting anymore");
    }
}
