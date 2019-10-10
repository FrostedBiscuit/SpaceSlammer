﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeShip : Enemy {

    public float BombTimer = 3f;
    public float FocusTimer = 3f;
    public float Speed = 30f;
    public float ChargeForce = 20f;
    public float ExplosionRadius = 3f;

    public Color PassiveCircleColor = Color.green;
    public Color AimingCircleColor = Color.yellow;
    public Color HotCircleColor = Color.red;

    bool focusingTarget = false;
    bool hot = false;

    [SerializeField]
    RadiusVisualizer ExplosionRadiusCircle = null;

    Coroutine expTimerCoroutine;

    public override void Dispose() {

        KamikazeShipPool.instance.ReturnObject(this);
    }

    protected override void OnEnable() {
        base.OnEnable();

        if (ExplosionRadiusCircle == null) {
            Debug.LogError("KamikazeShip::OnEnable() => No RadiusVisualizer found!!!");
        }
        else {
            ExplosionRadiusCircle.SetColor(PassiveCircleColor);
            ExplosionRadiusCircle.SetRange(AttackRange);
        }

        focusingTarget = false;
        hot = false;

        expTimerCoroutine = null;

        focusTimer = 0f;
    }

    protected override void FixedUpdate() {

        if (hot == true) {
            return;
        }

        base.FixedUpdate();

        move();
    }

    float focusTimer;

    protected override void Update() {

        if (hot == true) {
            return;
        }

        if (distanceToPlayer <= AttackRange) {

            focusingTarget = true;

            if (ExplosionRadiusCircle != null) {
                ExplosionRadiusCircle.SetColor(AimingCircleColor);
            }

            if (focusTimer == 0f) {

                focusTimer = Time.time + FocusTimer;

                rigidbody.velocity = Vector2.zero;
            }

        }

        if (focusTimer <= Time.time && focusTimer != 0f) {

            Attack();
        }
    }

    protected override void CheckForReposition() {

        if (hot == false) {
            base.CheckForReposition();
        }
    }

    protected override void Attack() {
        base.Attack();

        if (hot == true) {
            return;
        }

        if (ExplosionRadiusCircle != null) {
            ExplosionRadiusCircle.SetColor(HotCircleColor);
        }

        hot = true;

        float addedVel = ChargeForce * distanceToPlayer;

        Debug.Log($"Added velocity = {addedVel}, distance to player = {distanceToPlayer}");

        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(transform.up * addedVel, ForceMode2D.Impulse);

        //Debug.Log("added force = " + (transform.up * ChargeForce).ToString());

        expTimerCoroutine = StartCoroutine(explosionTimer());
    }

    protected override void Die() {
        base.Die();

        KamikazeShipPool.instance.ReturnObject(this);
    }

    private void move() {
        
        if (focusingTarget == false) { 
            
            rigidbody.AddForce(transform.up * Speed * Time.fixedDeltaTime);
        }

        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, Speed);
    }

    private void explode() {

        ParticlesPool.instance.RequestObject(transform.position, Quaternion.identity);

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        //Debug.Log("Boom!");

        for (int i = 0; i < cols.Length; i++) {

            if (cols[i].transform.tag == "Player") {
                Player.instance.TakeDamage(AttackDamage);
            }
            else if (cols[i] != null) {

                Enemy e = cols[i].gameObject.GetComponent<Enemy>();

                if (e != null) {

                    e.TakeDamage(AttackDamage);
                }
            }
        }
    }

    private void OnCollisionEnter2D() {

        if (hot == false) {
            return;
        }

        if (expTimerCoroutine != null) {
            StopCoroutine(expTimerCoroutine);
        }

        if (Sounds.Length > 0 && SoundManager.instance.PlaySFX == true) {

            SoundManager.instance.PlayRemoteSFXClip(Sounds[0], transform.position);
        }


        explode();

        Die();
    }

    bool isRunning = false;

    IEnumerator explosionTimer() {

        if (isRunning == true) {
            yield return null;
        }

        //Debug.Log("Explosion in " + BombTimer + " seconds");

        yield return new WaitForSeconds(BombTimer);

        explode();

        isRunning = false;

        Die();
    }

    protected override void OnDrawGizmosSelected() {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        if (rigidbody == null) {
            return;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, rigidbody.velocity);
    }
}
