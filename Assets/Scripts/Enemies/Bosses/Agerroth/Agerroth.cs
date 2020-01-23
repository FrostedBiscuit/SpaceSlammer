using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agerroth : Enemy {

    public float BackoffRange = 3.5f;
    public float SlopeCoefficient = 2.5f;
    public float RocketFiringPeriod = 1f;
    public float AttackCooldown = 3f;
    public float DodgeTime = 2f;
    public float DodgeCooldown = 5f;

    public int RocketFiringOnAttack = 3;
    public int DodgesBeforeCooldown = 3;

    public GameObject Rocket;

    public Transform RocketLauncherLeft;
    public Transform RocketLauncherRight;

    protected override void OnEnable() {
        base.OnEnable();

        UIBossHealthBar.instance.Enable();

        Rocket.GetComponent<AgerrothRocket>().Damage = AttackDamage;
    }

    protected override void Die() {
        base.Die();

        StopAllCoroutines();

        UIBossHealthBar.instance.Disable();

        Destroy(gameObject);
    }

    bool attacking = false;

    protected override void Attack() {
        base.Attack();

        if (attacking == true) {
            return;
        }

        StartCoroutine(fireRockets());

        attacking = true;
    }

    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, BackoffRange);
    }

    public override void Dispose() {

        UIBossHealthBar.instance.Disable();

        Destroy(gameObject);
    }

    public override void TakeDamage(float dmg) {
        base.TakeDamage(dmg);

        UIBossHealthBar.instance.UpdateHealth(currentHealth, MaxHealth);
    }

    protected override void Update() {
        
        if (distanceToPlayer <= AttackRange) {

            Attack();
        }

        if (lastDodgeTime < Time.time) {

            onDodgeCooldown = false;
        }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        move();
    }

    // Make this mess prettier
    float lastDodgeTime;

    int currDodges = 0;

    bool onDodgeCooldown = false;

    void move() {

        if (Player.instance.gameObject.activeSelf == true) {

            float force = 0f;

            if (distanceToPlayer > BackoffRange) {

                force = (distanceToPlayer - BackoffRange) * SlopeCoefficient;
            }
            else if (onDodgeCooldown == false) {

                force = -(BackoffRange - distanceToPlayer) * (SlopeCoefficient * 100f);

                // Dodge timing and counting
                if (lastDodgeTime < Time.time) {

                    currDodges++;

                    if (currDodges < DodgesBeforeCooldown) {

                        lastDodgeTime = DodgeTime + Time.time;
                    }
                    else {

                        lastDodgeTime = DodgeCooldown + Time.time;

                        currDodges = 0;

                        onDodgeCooldown = true;
                    }
                }
            }

            rigidbody.AddForce(transform.up * force * Time.fixedDeltaTime);

            Debug.DrawRay(transform.position, transform.up * force * Time.fixedDeltaTime);
        }

    }

    IEnumerator fireRockets() {

        //Debug.Log("Firing rockets");

        for (int i = 0; i < RocketFiringOnAttack; i++) {

            Rigidbody2D rocketRBLeft = Instantiate(Rocket, RocketLauncherLeft.position, RocketLauncherLeft.rotation).GetComponent<Rigidbody2D>();
            Rigidbody2D rocketRBRight = Instantiate(Rocket, RocketLauncherRight.position, RocketLauncherRight.rotation).GetComponent<Rigidbody2D>();

            rocketRBLeft.AddForce(-RocketLauncherLeft.right * 150f);
            rocketRBRight.AddForce(RocketLauncherLeft.right * 150f);

            yield return new WaitForSeconds(RocketFiringPeriod);
        }

        yield return new WaitForSeconds(AttackCooldown);

        attacking = false;
    }
 }
