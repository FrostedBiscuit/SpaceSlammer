using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WeakPoint : MonoBehaviour {

    public float MaxHealth;
    public float WeakPointDamageMultiplier = 10f;

    public float CurrentHealth {
        get {
            return currentHealth;
        }
    }

    float currentHealth;

    public void TakeDamage(float dmg) {

        if (currentHealth - dmg > 0f) {

            currentHealth -= dmg;
        }
        else {

            currentHealth = 0f;

            Destroy(gameObject);
        }
    }

    private void Start() {

        currentHealth = MaxHealth;
    }
}
