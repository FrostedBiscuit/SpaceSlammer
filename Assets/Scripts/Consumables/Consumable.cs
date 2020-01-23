using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class Consumable : MonoBehaviour {

    public float Amount = 0f;
    public float Duration = 0f;
    public float MaxDistanceFromPlayer = 15f;
    public float RepositionCheckInterval = 5f;

    Action<Consumable> onConsumeCallback;

    public abstract void ConsumableEffect();

    public void RegisterOnConsumeCallback(Action<Consumable> cb) {

        onConsumeCallback += cb;
    }

    public void UnregisterOnConsumeCallback(Action<Consumable> cb) {

        onConsumeCallback -= cb;
    }

    private void Start() {

        InvokeRepeating("checkForReposition", RepositionCheckInterval, RepositionCheckInterval);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.transform.tag == "Player") {

            if (onConsumeCallback != null) {

                onConsumeCallback(this);
            }

            CancelInvoke();

            ConsumableEffect();
        }
    }

    private void OnDisable() {

        onConsumeCallback = null;
    }

    private void checkForReposition() {

        if (Player.instance.gameObject.activeSelf == false) {
            return;
        }

        float distance = Vector3.Distance(transform.position, Player.instance.transform.position);

        if (distance > MaxDistanceFromPlayer) {

            float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

            Vector3 newOffset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * UnityEngine.Random.Range(MaxDistanceFromPlayer * 0.4f, MaxDistanceFromPlayer);

            transform.position = Player.instance.transform.position + newOffset;
        }
    }

    private void OnDrawGizmosSelected() {

        if (Player.instance.gameObject.activeSelf == false) {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Player.instance.transform.position, MaxDistanceFromPlayer);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Player.instance.transform.position, MaxDistanceFromPlayer * 0.4f);
    }
}
