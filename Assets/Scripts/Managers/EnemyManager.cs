using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singelton
    public static EnemyManager instance;

    private void Awake() {
        
        if (instance != null) {
            Debug.LogError("EnemyManager::Awake() => More than 1 instance of EnemyManager in the scene!!!");
            return;
        }

        instance = this;
    }
    #endregion

    public float EnemySpawnNearDist = 5f;
    public float EnemySpawnFarDist = 10f;

    public int MinEnemyNum = 1;
    public int MaxEnemyNum = 4;
    public int sumEnemyDied;

    public List<PooledEnemy> Enemies = new List<PooledEnemy>();

    bool canSpawn = false;

    List<Enemy> activeEnemies = new List<Enemy>();

    public void StartSpawning() {

        ClearEnemies();

        canSpawn = true;

        sumEnemyDied = 0;

        spawnNumEnemies(Random.Range(MinEnemyNum, MaxEnemyNum));
    }

    public void EndSpawning() {

        canSpawn = false;
    }

    public void ClearEnemies() {

        if (activeEnemies.Count == 0) return;

        for (int i = 0; i < activeEnemies.Count; i++) {

            ObjectPool.instance.ReturnObject(activeEnemies[i].gameObject);
        }

        activeEnemies.Clear();
    }

    // TEST CODE FOR GATHERING IDEAS!!!!
    // PRONE TO MUCH CHANGE
    private void spawnNumEnemies(int enemyCount) {

        Debug.Log("spawnNumEnemies");

        for (int i = 0; i < enemyCount; i++) {

            int randomIndex = Random.Range(0, Enemies.Count);
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);

            Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(EnemySpawnNearDist, EnemySpawnFarDist);

            Enemy e = ObjectPool.instance.RequestObject(Enemies[randomIndex].Enemy.gameObject, offset, Quaternion.identity).GetComponent<Enemy>();
            e.RegisterOnDeathCallback(onEnemyDeath);

            activeEnemies.Add(e);
        }
    }

    // TEST CODE FOR GATHERING IDEAS!!!!
    // PRONE TO MUCH CHANGE
    void onEnemyDeath(Enemy e) {

        e.UnregisterOnDeathCallback(onEnemyDeath);

        activeEnemies.Remove(e);

        sumEnemyDied += e.ScoreValue;

        if (activeEnemies.Count == 0 && canSpawn == true) {

            activeEnemies.Clear();

            spawnNumEnemies(Random.Range(MinEnemyNum, MaxEnemyNum));
        }
    }

    [System.Serializable]
    public struct PooledEnemy {

        public Enemy Enemy;
    }
}
