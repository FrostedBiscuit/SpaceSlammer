using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public enum Effect {
        HEAL, DAMAGEBOOST, INVINCIBILITY, STUN
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

    float invincibilityTime = 0f;
    float stunnedTime = 0f;

    // Update is called once per frame
    void Update() {

        invincibilityTime = Mathf.Clamp(invincibilityTime - Time.deltaTime, 0f, float.MaxValue);
        stunnedTime = Mathf.Clamp(stunnedTime - Time.deltaTime, 0f, float.MaxValue);

        if (Player.instance.gameObject.activeSelf == false) {
            // Player has died, do something

            EnemyManager.instance.EndSpawning();

            ConsumablesManager.instance.StopSpawningConsumables();

            UIManager.instance.ActivateEndScreen();
        }
        else {

            Player.instance.CanTakeDamage = invincibilityTime > 0f ? false : true;
            Player.instance.CanMove = stunnedTime > 0f ? false : true;

            if (Player.instance.CanTakeDamage == false) {

                UIInvincibilityIcon.instance.Activate();
            }
            else {

                UIInvincibilityIcon.instance?.Deactivate();
            }
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

    public void ApplyEffect(Effect effect, float amount = 0f, float duration = 0f) {

        switch(effect) {
            case Effect.DAMAGEBOOST:
                StartCoroutine(boostPlayerDamageForSeconds(amount, duration));
            break;
            case Effect.HEAL:
                Player.instance.Health += amount;
            break;
            case Effect.INVINCIBILITY:
                playerInvincibleForSeconds(duration);
            break;
            case Effect.STUN:
                playerStunnedForSeconds(duration);
            break;
        }
    }

    IEnumerator boostPlayerDamageForSeconds(float amount, float duration) {

        UIDamageBoostIcon.instance.Activate();

        Player.instance.DamageMultiplier += amount;
        Player.instance.PlayerVFX.Enable(Effect.DAMAGEBOOST, duration);

        yield return new WaitForSeconds(duration);

        Player.instance.DamageMultiplier -= amount;
        Player.instance.DamageMultiplier = Mathf.Clamp(Player.instance.DamageMultiplier, 1f, Mathf.Infinity);

        if (Player.instance.DamageMultiplier == 1f) {

            UIDamageBoostIcon.instance.Deactivate();
        }
    }

    void playerInvincibleForSeconds(float duration) {

        invincibilityTime += duration;

        Player.instance.PlayerVFX.Enable(Effect.INVINCIBILITY, duration);
    }

    void playerStunnedForSeconds(float duration) {

        stunnedTime += duration;
    }
}
