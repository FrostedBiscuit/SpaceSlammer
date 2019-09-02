using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Mine : MonoBehaviour {

    [SerializeField]
    private float Damage = 10f;
    [SerializeField]
    private float ExplosionRadius = 3f;
    [SerializeField]
    private float Timer = 3f;
    [SerializeField]
    private float MaxDistanceFromPlayer = 40f;

    [SerializeField]
    private GameObject ExplosionParticles = null;

    private float distanceCheckInterval = 5f;

    private Action<Mine> mineExplosionCallback;

    private void OnEnable() {

        hot = false;

        InvokeRepeating("checkForReposition", distanceCheckInterval, distanceCheckInterval);
    }

    private void checkForReposition() {

        if (Player.instance.gameObject.activeSelf == true && hot == false) {
            
            float distanceToPlayer = Vector3.Distance(transform.position, Player.instance.transform.position);

            if (distanceToPlayer > MaxDistanceFromPlayer) {

                float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

                Vector3 newPos = Player.instance.transform.position +
                                 new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * UnityEngine.Random.Range(EnemyManager.instance.EnemySpawnNearDist, EnemyManager.instance.EnemySpawnFarDist);

                transform.position = newPos;
            }
        }
    }

    bool hot = false;

    private void OnCollisionEnter2D() {
        
        if (hot == true) {
            return;
        }

        hot = true;

        StartCoroutine(ExplosionTimer());
    }

    IEnumerator ExplosionTimer() {

        yield return new WaitForSeconds(Timer);

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        for (int i = 0; i < cols.Length; i++) {

            if (cols[i].name == name) {
                continue;
            }

            if (cols[i].name == "Player") {
                Player.instance.TakeDamage(Damage);
            }

            Enemy e = cols[i].GetComponent<Enemy>();

            if (e != null) {

                e.TakeDamage(Damage);
            }
        }

        CancelInvoke();

        if (mineExplosionCallback != null) {
            mineExplosionCallback(this);
        }

        ObjectPool.instance.RequestObject(ExplosionParticles, transform.position, transform.rotation);
        //ObjectPool.instance.ReturnObject(gameObject);

        Destroy(gameObject);
    }

    public void RegisterOnExplosionCallback(Action<Mine> cb) {

        mineExplosionCallback += cb;
    }

    public void UnregisterOnExplosionCallback(Action<Mine> cb) {

        mineExplosionCallback -= cb;
    }
}
