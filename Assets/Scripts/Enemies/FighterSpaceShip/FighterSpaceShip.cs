using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSpaceShip : Enemy {

    public float BackoffRange = 1.5f;
    public float SlopeCoefficient = 2f;
    public float WanderingSpeed = 50f;
    public float FireRate = 1f;

    [SerializeField]
    Transform ProjectileSpawnPoint = null;

    float nextFire;

    protected override void Attack() {
        base.Attack();
#if UNITY_EDITOR
        if (DEBUG_Fight == false) {
            return;
        }
#endif        
        nextFire = FireRate + Time.time;

        //Debug.Log("Fighter ship attacking");

        ProjectilePool.instance.RequestObject(ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);
    }

    protected override void Die() {
        base.Die();

        FighterSpaceShipPool.instance.ReturnObject(this);
    }

    protected override void OnEnable() {
        base.OnEnable();

        if (ProjectileSpawnPoint == null) {
            Debug.LogError("BasicSpaceShip::Start() => No projectile spawn point found!!!");
        }

        nextFire = Time.time + FireRate;
    }

    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, BackoffRange);

        if (rigidbody == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, rigidbody.velocity);
    }

    public override void TakeDamage(float dmg) {
        base.TakeDamage(dmg);

        //Debug.Log("BasicSpaceShip::TakeDamage() => Damage taken: " + dmg);
    }

    public override void Dispose() {

        FighterSpaceShipPool.instance.ReturnObject(this);
    }

    protected override void Update() {

        if (nextFire < Time.time                          && 
            Player.instance.gameObject.activeSelf == true && 
            distanceToPlayer < AttackRange) {
            Attack();
        }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        move();
    }

    void move() {

        if (Player.instance.gameObject.activeSelf == true) {

            float speed = SlopeCoefficient * distanceToPlayer - SlopeCoefficient * BackoffRange;

            //Debug.Log($"Speed: {speed}, distance to player: {distanceToPlayer}");

            rigidbody.AddForce(transform.up * speed * Time.fixedDeltaTime);
        }
        else {
            rigidbody.AddForce(transform.up * WanderingSpeed * Time.fixedDeltaTime);
        }

        //rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, Max);
    }
}
