using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region Singelton
    public static Player instance;

    private void Awake() {

        if (instance != null)
            Debug.LogError("Player::Awake() => More than 1 player in the scene!!!");

        instance = this;
    }
    #endregion

    public float Speed = 10f;
    public float FollowSpeed = 1.5f;
    public float MaxHealth = 50f;

    [SerializeField]
    Rigidbody2D Rigidbody = null;

    [HideInInspector]
    public float Health;

    // Start is called before the first frame update
    void Start() {
        if (Rigidbody == null) {
            Debug.LogError("Player::Start() => No rigidbody reference found!!");
        }

        Health = MaxHealth;
    }

    public void TakeDamage(float dmg) {

        if (Health <= dmg) {
            // TODO: change this to something less shitty
            Destroy(gameObject);
        }
        else {
            Health -= dmg;
        }
    }

    public Rigidbody2D GetRigidbody() {

        return Rigidbody;
    }
}
