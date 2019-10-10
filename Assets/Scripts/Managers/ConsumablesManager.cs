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
    private float MinSpawnCycleDelay = 5f;
    [SerializeField]
    private float MaxSpawnCycleDelay = 10f;
    [SerializeField]
    private float MaxSpawnDistanceFromLastConsumable = 20f;

    private List<DamageBooster> activeDamageBoosters = new List<DamageBooster>();
    private List<HealthPickup> activeHealthPickups = new List<HealthPickup>();

    public void StartSpawningConsumables() {

        Invoke("spawnRandomConsumable", Random.Range(MinSpawnCycleDelay, MaxSpawnCycleDelay));
    }

    public void StopSpawningConsumables() {

        CancelInvoke();

        activeDamageBoosters.Clear();
        activeHealthPickups.Clear();
    }

    public void ClearConsumables() { 

        foreach (var db in activeDamageBoosters) {

            DamageBoosterPool.instance.ReturnObject(db);
        }

        foreach (var hp in activeHealthPickups) {

            HealthPickupPool.instance.ReturnObject(hp);
        }

        activeDamageBoosters.Clear();
        activeHealthPickups.Clear();
    }

    Vector3 lastPos;

    private void spawnRandomConsumable() {

        if (activeDamageBoosters.Count >= MaxActiveDamageBoosters && activeHealthPickups.Count >= MaxActiveHealthPickups) {
            return;
        }

        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        Vector3 newPos = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(MaxSpawnDistanceFromLastConsumable * 0.4f, MaxSpawnDistanceFromLastConsumable);

        if (activeDamageBoosters.Count < MaxActiveDamageBoosters && activeHealthPickups.Count < MaxActiveHealthPickups) {

            int random01 = Random.Range(0, 2);

            //Debug.Log($"Random number in consumable manager: {random01}");

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
    }

    private void onDamageBoosterConsumed(Consumable c) {

        c.UnregisterOnConsumeCallback(onDamageBoosterConsumed);

        activeDamageBoosters.Remove((DamageBooster)c);
    }

    private void onHealthPickupConsumed(Consumable c) {

        c.UnregisterOnConsumeCallback(onHealthPickupConsumed);

        activeHealthPickups.Remove((HealthPickup)c);
    }
}
