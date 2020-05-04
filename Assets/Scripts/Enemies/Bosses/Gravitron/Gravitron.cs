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

    public GravitronProjectile Projectile = null;

    bool isTargeting = false;

    protected override void OnEnable() {
        base.OnEnable();

        UIBossHealthBar.instance.Enable(Name);

        isTargeting = false;
    }

    public override void TakeDamage(float dmg) {
        base.TakeDamage(dmg);

        UIBossHealthBar.instance.UpdateHealth(currentHealth, MaxHealth);
    }

    public override void Dispose() {

        UIBossHealthBar.instance.Disable();

        Destroy(gameObject);
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        move();
    }

    protected override void Attack() {
        base.Attack();

        Projectile.Damage = AttackDamage;

        Instantiate(Projectile.gameObject, Gun.position, Gun.rotation);
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

        ExplosionParticlesPool.instance.RequestObject(transform.position, transform.rotation);

        UIBossHealthBar.instance.Disable();

        Destroy(gameObject);
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

        for (int i = 0; i < TimesToFire; i++) {

            Attack();

            yield return new WaitForSeconds(TargetTime / TimesToFire);
        }

        isTargeting = false;
    }
}
