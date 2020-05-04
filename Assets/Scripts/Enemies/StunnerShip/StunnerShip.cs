using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnerShip : Enemy {

    // TODO:
    // - Make projectile
    // - Get sounds
    // - Set up sounds
    // - ...
    // - Profit?

    public float AttackCooldown = 1f;
    public float BackoffRange = 3.5f;
    public float WanderingSpeed = 50f;
    public float SlopeCoefficient = 200f;
    public float ShipFOV = 45f;

    [SerializeField]
    Transform ProjectileLauncher;

    [SerializeField]
    Animator Animator;

    float lastShot;

    private const string AttackTriggerKey = "Attack";

    protected override void OnEnable() {
        base.OnEnable();

        // Maybe reset certain values like 'lastFired' or stn...

        lastShot = -1f;
    }

    protected override void Attack() {
        base.Attack();

        StunnerShipProjectilePool.instance.RequestObject(ProjectileLauncher.position, ProjectileLauncher.rotation).transform.SetParent(ProjectileLauncher);

        Animator.ResetTrigger(AttackTriggerKey);
    }

    protected override void Die() {
        base.Die();

        ExplosionParticlesPool.instance.RequestObject(transform.position, transform.rotation);

        StunnerShipPool.instance.ReturnObject(this);
    }

    public override void Dispose() {

        // Return object to object pool

        StunnerShipPool.instance.ReturnObject(this);
    }

    protected override void Update() {

        // Check for player proximity and last shot time
        // to guage whether to attack or not.
        // Attack when player is in range.
        // If player is out of range, attacking is impossible.

        Vector3 dirToPlayer = (pointOfInterest - transform.position).normalized;

        if (distanceToPlayer < AttackRange && 
            lastShot < Time.time &&
            Mathf.Abs((Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg) - (Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg)) <= (ShipFOV / 2f)) {
            // TODO: fix angle of attack

            Animator.SetTrigger(AttackTriggerKey);

            lastShot = AttackCooldown + Time.time;
        }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        move();
    }

    private void move() {

        if (Player.instance.gameObject.activeSelf == true) {

            float speed = SlopeCoefficient * distanceToPlayer - SlopeCoefficient * BackoffRange;

            //Debug.Log($"Speed: {speed}, distance to player: {distanceToPlayer}");

            rigidbody.AddForce(transform.up * speed * Time.fixedDeltaTime);
        }
        else {
            rigidbody.AddForce(transform.up * WanderingSpeed * Time.fixedDeltaTime);
        }
    }

    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, BackoffRange);

        float currAngle = -Mathf.Atan2(transform.up.y, transform.up.x)  + (Mathf.PI / 2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(Mathf.Sin(currAngle - (ShipFOV * Mathf.Deg2Rad / 2f)), Mathf.Cos(currAngle - (ShipFOV * Mathf.Deg2Rad / 2f))) * AttackRange));
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(Mathf.Sin(currAngle - (-ShipFOV * Mathf.Deg2Rad / 2f)), Mathf.Cos(currAngle - (-ShipFOV * Mathf.Deg2Rad / 2f))) * AttackRange));

        Vector3 dir = (pointOfInterest - transform.position).normalized;

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.position + (new Vector3(Mathf.Sin(rigidbody.rotation), Mathf.Cos(rigidbody.rotation)) * AttackRange));
    }
}
