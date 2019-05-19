using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyWeaponsShip : Enemy
{
    public float BackoffRange = 3f;
    public float BeamDuration = 2f;
    public float BeamLenght = 3f;
    public float BeamDamageInterval = 0.25f;

    [SerializeField]
    GameObject Beam = null;

    protected override void Start() {
        base.Start();

        Beam.SetActive(false);
    }

    float beamCurrentLifetime;

    protected override void Attack() {
        base.Attack();

        if (distanceToPlayer <= AttackRange && beamCurrentLifetime <= Time.time) {

            Beam.SetActive(true);

            beamCurrentLifetime = Time.time + BeamDuration;

            Debug.Log("Firing beam");
        }
    }

    protected override void Update() {

        if (Player.instance != null) pointOfInterest = Player.instance.transform.position;

        Attack();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    protected override void Die() {
        base.Die();

        Destroy(gameObject);
    }
}
