using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeaponsShip : Enemy
{
    public float BackoffRange = 3f;
    public float SlopeCoefficient = 150f;
    public float WanderingSpeed = 10f;
    public float BeamCooldown = 5f;
    public float BeamDuration = 2f;
    public float BeamLenght = 3f;
    public float BeamDamageInterval = 0.25f;

    [SerializeField]
    GameObject Beam = null;

    public override void Dispose() {

        HeavyWeaponsShipPool.instance.ReturnObject(this);
    }

    protected override void OnEnable() {
        base.OnEnable();

        Beam.SetActive(false);
    }

    float beamCurrentCooldown;
    float beamCurrentLifetime;

    protected override void Attack() {
        base.Attack();

        Beam.SetActive(true);

        beamCurrentCooldown = Time.time + BeamCooldown;

        beamCurrentLifetime = Time.time + BeamDuration;
    }

    protected override void Update() {

        if (Player.instance.gameObject.activeSelf == true) pointOfInterest = Player.instance.transform.position;

        if (beamCurrentLifetime < Time.time               &&
            beamCurrentCooldown < Time.time               &&
            Player.instance.gameObject.activeSelf == true &&
            distanceToPlayer < AttackRange                &&
            Beam.activeSelf == false) {


            Debug.Log($"attacking distance to player: {distanceToPlayer}");
            Attack();
        }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        move();
    }

    protected override void Die() {
        base.Die();

        HeavyWeaponsShipPool.instance.ReturnObject(this);
    }

    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();

        if (rigidbody == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, rigidbody.velocity);
    }

    void move() {

        if (Player.instance.gameObject.activeSelf == true) {

            Debug.Log("player active");

            if (distanceToPlayer < AttackRange) {

                rigidbody.velocity = Vector2.zero;
            }
            else {

                float speed = SlopeCoefficient * distanceToPlayer - SlopeCoefficient * BackoffRange;

                rigidbody.AddForce(transform.up * speed * Time.fixedDeltaTime);
            }
        }
        else {
            rigidbody.AddForce(transform.up * WanderingSpeed * Time.fixedDeltaTime);
        }
    }
}
