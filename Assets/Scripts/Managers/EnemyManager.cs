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
    public float MineCheckInterval = 5f;

    public int MinEnemyNum = 1;
    public int MaxEnemyNum = 4;
    public int MaxMines = 5;
    public int sumEnemyDied;

    public List<PooledEnemy> Enemies = new List<PooledEnemy>();

    public GameObject Mine = null;

    bool canSpawn = false;

    int currentMines;

    List<Enemy> activeEnemies = new List<Enemy>();
    List<Mine> activeMines = new List<Mine>();

    public void StartSpawning() {

        ClearEnemies();

        canSpawn = true;

        sumEnemyDied = 0;

        spawnNumEnemies(Random.Range(MinEnemyNum, MaxEnemyNum));

        InvokeRepeating("spawnMines", 0f, MineCheckInterval);
    }

    public void EndSpawning() {

        CancelInvoke("spawnMines");

        canSpawn = false;
    }

    public void ClearEnemies() {

        if (activeEnemies.Count == 0) return;

        for (int i = 0; i < activeEnemies.Count; i++) {

            ObjectPool.instance.ReturnObject(activeEnemies[i].gameObject);
        }

        activeEnemies.Clear();

        clearMines();
    }

    private void spawnMines() {

        if (currentMines == 0) {

            for (int i = 0; i < MaxMines; i++) {

                float randomAngle = Random.Range(0f, Mathf.PI * 2f);
                Vector3 newPos = Player.instance.transform.position + 
                                 new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(EnemySpawnNearDist, EnemySpawnFarDist);

                Mine m = ObjectPool.instance.RequestObject(Mine, newPos, Quaternion.identity).GetComponent<Mine>();
                m.RegisterOnExplosionCallback(onMineExplosion);

                activeMines.Add(m);

                currentMines++;
            }
        }
        else if (currentMines < MaxMines) {

            int mineDelta = MaxMines - currentMines;
            int numMinesToSpawn = Random.Range(0, mineDelta);

            for (int i = 0; i < numMinesToSpawn; i++) {

                float randomAngle = Random.Range(0f, Mathf.PI * 2f);
                Vector3 newPos = Player.instance.transform.position +
                                 new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(EnemySpawnNearDist, EnemySpawnFarDist);

                Mine m = ObjectPool.instance.RequestObject(Mine, newPos, Quaternion.identity).GetComponent<Mine>();
                m.RegisterOnExplosionCallback(onMineExplosion);

                activeMines.Add(m);

                currentMines++;
            }
        }
    }

    private void clearMines() {

        for (int i = 0; i < activeMines.Count; i++) {

            ObjectPool.instance.ReturnObject(activeMines[i].gameObject);
        }
    }

    // TEST CODE FOR GATHERING IDEAS!!!!
    // PRONE TO MUCH CHANGE
    private void spawnNumEnemies(int enemyCount) {

        //Debug.Log("spawnNumEnemies");

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

    void onMineExplosion(Mine m) {

        m.UnregisterOnExplosionCallback(onMineExplosion);

        currentMines--;

        if (currentMines < 0) {
            Debug.LogError($"EnemyManager::onMineExplosion() => currentMines is less than 0!!! currentMines: {currentMines}");
        }
    }

    [System.Serializable]
    public struct PooledEnemy {

        public Enemy Enemy;
    }
}
