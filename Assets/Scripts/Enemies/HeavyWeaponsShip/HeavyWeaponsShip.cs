using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeaponsShip : Enemy
{
    public float BackoffRange = 3f;
    public float Speed = 10f;
    public float BeamDuration = 2f;
    public float BeamLenght = 3f;
    public float BeamDamageInterval = 0.25f;

    [SerializeField]
    GameObject Beam = null;

    protected override void OnEnable() {
        base.OnEnable();

        Beam.SetActive(false);

        initialRotationSmoothing = RotationSmoothing;
    }

    float beamCurrentLifetime;
    float initialRotationSmoothing;

    protected override void Attack() {
        base.Attack();

        if (distanceToPlayer <= AttackRange && beamCurrentLifetime <= Time.time) {

            Beam.SetActive(true);

            beamCurrentLifetime = Time.time + BeamDuration;

            RotationSmoothing = RotationSmoothing * 10f;
        }
        else {
            RotationSmoothing = initialRotationSmoothing;
        }
    }

    protected override void Update() {

        if (Player.instance.gameObject.activeSelf == true) pointOfInterest = Player.instance.transform.position;

        Attack();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        move();
    }

    protected override void Die() {
        base.Die();

        ObjectPool.instance.ReturnObject(gameObject);
    }

    void move() {
        
        if (distanceToPlayer < AttackRange) {

            rigidbody.velocity = Vector2.zero;
        }
        else {

            rigidbody.velocity = transform.up * Speed * Time.fixedDeltaTime;
        }
    }
}
