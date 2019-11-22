using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public enum Effect {
        HEAL, DAMAGEBOOST, INVINCIBILITY
    }

    #region Singelton
    public static PlayerManager instance;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("PlayerManager::Awake() => More than 1 instance of PlayerManager in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField]
    GameObject PlayerGO = null;

    [SerializeField]
    Transform Spawn = null;

    // Start is called before the first frame update
    void Start() {

        SpawnPlayer();
    }

    // Update is called once per frame
    void Update() {

        if (Player.instance.gameObject.activeSelf == false) {
            // Player has died, do something

            EnemyManager.instance.EndSpawning();

            ConsumablesManager.instance.StopSpawningConsumables();

            UIManager.instance.ActivateEndScreen();
        }
    }

    public void SpawnPlayer() {

        if (Player.instance.gameObject.activeSelf == true) {
            return;
        }

        PlayerGO.SetActive(true);
        PlayerGO.transform.position = Spawn.position;
        PlayerGO.transform.rotation = Quaternion.identity;
    }

    public void DespawnPlayer() {

        if (Player.instance.gameObject.activeSelf == false) {
            return;
        }

        StopAllCoroutines();

        PlayerGO.SetActive(false);
    }

    public void ApplyEffect(Effect effect, float amount, float duration = 0f) {

        switch(effect) {
            case Effect.DAMAGEBOOST:
                StartCoroutine(boostPlayerDamageForSeconds(amount, duration));
            break;
            case Effect.HEAL:
                Player.instance.Health += amount;
            break;
            case Effect.INVINCIBILITY:
                StartCoroutine(playerInvincibleForSeconds(duration));
            break;
        }
    }

    IEnumerator boostPlayerDamageForSeconds(float amount, float duration) {
        
        Player.instance.DamageMultiplier += amount;

        yield return new WaitForSeconds(duration);

        Player.instance.DamageMultiplier -= amount;
        Player.instance.DamageMultiplier = Mathf.Clamp(Player.instance.DamageMultiplier, 1f, Mathf.Infinity);
    }

    IEnumerator playerInvincibleForSeconds(float duration) {

        Player.instance.CanTakeDamage = false;

        Debug.Log("invincibility started");

        yield return new WaitForSeconds(duration);

        Player.instance.CanTakeDamage = true;

        Debug.Log("invincibility wore off");
    }
}
