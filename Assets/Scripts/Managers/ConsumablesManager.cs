using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumablesManager : MonoBehaviour {

    #region Singelton
    public static ConsumablesManager instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("ConsumablesManager::Awake() => More than 1 instance of ConsumablesManager in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField]
    private int MaxActiveDamageBoosters = 4;
    [SerializeField]
    private int MaxActiveHealthPickups = 4;
    [SerializeField]
    private int MaxActiveInvincibilityPickups = 2;

    [SerializeField]
    private float MinSpawnCycleDelay = 5f;
    [SerializeField]
    private float MaxSpawnCycleDelay = 10f;
    [SerializeField]
    private float MaxSpawnDistanceFromLastConsumable = 20f;

    private List<DamageBooster> activeDamageBoosters = new List<DamageBooster>();
    private List<HealthPickup> activeHealthPickups = new List<HealthPickup>();
    private List<InvincibilityPickup> activeInvincibilityPickups = new List<InvincibilityPickup>();

    public void StartSpawningConsumables() {

        Invoke("spawnRandomConsumable", Random.Range(MinSpawnCycleDelay, MaxSpawnCycleDelay));
    }

    public void StopSpawningConsumables() {

        CancelInvoke();
    }

    public void ClearConsumables() {

        foreach (var db in activeDamageBoosters) {

            DamageBoosterPool.instance.ReturnObject(db);
        }

        foreach (var hp in activeHealthPickups) {

            HealthPickupPool.instance.ReturnObject(hp);
        }

        foreach (var iv in activeInvincibilityPickups) {

            InvincibilityPickupPool.instance.ReturnObject(iv);
        }

        activeDamageBoosters.Clear();
        activeHealthPickups.Clear();
        activeInvincibilityPickups.Clear();
    }

    Vector3 lastPos;

    private void spawnRandomConsumable() {

        int randomConsumableIndex = Random.Range(0, 4);

        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        Vector3 newPos = lastPos + new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(MaxSpawnDistanceFromLastConsumable * 0.4f, MaxSpawnDistanceFromLastConsumable);

        lastPos = newPos;

        switch (randomConsumableIndex) {
            case 0:
                spawnDamangeBooster(newPos);
            break;
            case 1:
                spawnHealthPickup(newPos);
            break;
            case 2:
                spawnInvincibilityPickup(newPos);
            break;
        }

        Invoke("spawnRandomConsumable", Random.Range(MinSpawnCycleDelay, MaxSpawnCycleDelay));
    }

    /*private void spawnRandomConsumable1() {

        if (activeDamageBoosters.Count >= MaxActiveDamageBoosters && activeHealthPickups.Count >= MaxActiveHealthPickups) {
            return;
        }

        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        Vector3 newPos = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(MaxSpawnDistanceFromLastConsumable * 0.4f, MaxSpawnDistanceFromLastConsumable);

        if (activeDamageBoosters.Count < MaxActiveDamageBoosters && activeHealthPickups.Count < MaxActiveHealthPickups) {

            int random01 = Random.Range(0, 2);

            if (random01 == 1) {

                DamageBooster db = DamageBoosterPool.instance.RequestObject(lastPos + newPos, Quaternion.identity);
                db.RegisterOnConsumeCallback(onDamageBoosterConsumed);

                activeDamageBoosters.Add(db);
            }
            else {

                HealthPickup hp = HealthPickupPool.instance.RequestObject(lastPos + newPos, Quaternion.identity);
                hp.RegisterOnConsumeCallback(onHealthPickupConsumed);

                activeHealthPickups.Add(hp);
            }
        }
        else if (activeDamageBoosters.Count >= MaxActiveDamageBoosters && activeHealthPickups.Count < MaxActiveHealthPickups) {

            HealthPickup hp = HealthPickupPool.instance.RequestObject(lastPos + newPos, Quaternion.identity);
            hp.RegisterOnConsumeCallback(onHealthPickupConsumed);

            activeHealthPickups.Add(hp);
        }
        else if (activeDamageBoosters.Count < MaxActiveDamageBoosters && activeHealthPickups.Count >= MaxActiveHealthPickups) {

            DamageBooster db = DamageBoosterPool.instance.RequestObject(lastPos + newPos, Quaternion.identity);
            db.RegisterOnConsumeCallback(onDamageBoosterConsumed);

            activeDamageBoosters.Add(db);
        }

        //Debug.Log($"Active damage boosters: {activeDamageBoosters.Count}, active health pickups: {activeHealthPickups.Count}");

        lastPos = newPos;
        
        Invoke("spawnRandomConsumable", Random.Range(MinSpawnCycleDelay, MaxSpawnCycleDelay));
    }*/

    private void onDamageBoosterConsumed(Consumable c) {

        c.UnregisterOnConsumeCallback(onDamageBoosterConsumed);

        activeDamageBoosters.Remove((DamageBooster)c);
    }

    private void onHealthPickupConsumed(Consumable c) {

        c.UnregisterOnConsumeCallback(onHealthPickupConsumed);

        activeHealthPickups.Remove((HealthPickup)c);
    }

    private void onInvincibilityPickupConsumed(Consumable c) {

        c.UnregisterOnConsumeCallback(onInvincibilityPickupConsumed);

        activeInvincibilityPickups.Remove((InvincibilityPickup)c);
    }

    private void spawnDamangeBooster(Vector3 pos) {

        if (activeDamageBoosters.Count <= MaxActiveDamageBoosters) {

            DamageBooster db = DamageBoosterPool.instance.RequestObject(pos, Quaternion.identity);
            db.RegisterOnConsumeCallback(onDamageBoosterConsumed);

            Debug.Log("Spawned damage booster");

            activeDamageBoosters.Add(db);
        }
    }

    private void spawnHealthPickup(Vector3 pos) {

        if (activeHealthPickups.Count <= MaxActiveHealthPickups) {

            HealthPickup hp = HealthPickupPool.instance.RequestObject(pos, Quaternion.identity);
            hp.RegisterOnConsumeCallback(onHealthPickupConsumed);

            Debug.Log("Spawned health pickup");

            activeHealthPickups.Add(hp);
        }
    }

    private void spawnInvincibilityPickup(Vector3 pos) {

        if (activeInvincibilityPickups.Count <= MaxActiveInvincibilityPickups) {

            InvincibilityPickup iv = InvincibilityPickupPool.instance.RequestObject(pos, Quaternion.identity);
            iv.RegisterOnConsumeCallback(onInvincibilityPickupConsumed);

            Debug.Log("Spawned invincibility pickup");

            activeInvincibilityPickups.Add(iv);
        }
    }
}
