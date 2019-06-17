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

    Vector3 inactiveCoords = new Vector3(0f, 0f, 20f);

    List<Enemy> inactiveEnemies = new List<Enemy>();
    List<Enemy> activeEnemies = new List<Enemy>();

    [SerializeField]
    Transform PooledEnemyParent = null;

    // Start is called before the first frame update
    void Start() {
        
        foreach(PooledEnemy pe in Enemies) {
            for (int i = 0; i < pe.NumToSpawn; i++) {

                Enemy e = Instantiate(pe.Enemy.gameObject, inactiveCoords, Quaternion.identity, PooledEnemyParent).GetComponent<Enemy>();
                e.gameObject.SetActive(false);

                inactiveEnemies.Add(e);
            }
        }
    }

    bool canSpawn = false;

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

        for (int i = activeEnemies.Count - 1; i >= 0; i--) {

            DespawnEnemy(activeEnemies[i]);
        }
    }

    // TEST CODE FOR GATHERING IDEAS!!!!
    // PRONE TO MUCH CHANGE
    private void spawnNumEnemies(int enemyCount) {

        for (int i = 0; i < enemyCount; i++) {

            int randomIndex = Random.Range(0, Enemies.Count);
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);

            Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(EnemySpawnNearDist, EnemySpawnFarDist); 

            SpawnEnemy(Enemies[randomIndex].Enemy, offset, Quaternion.identity).RegisterOnDeathCallback(onEnemyDeath);
        }
    }

    // TEST CODE FOR GATHERING IDEAS!!!!
    // PRONE TO MUCH CHANGE
    void onEnemyDeath(Enemy e) {

        e.UnregisterOnDeathCallback(onEnemyDeath);

        activeEnemies.Remove(e);

        sumEnemyDied += e.ScoreValue;

        if (activeEnemies.Count == 0 && canSpawn == true) {

            spawnNumEnemies(Random.Range(MinEnemyNum, MaxEnemyNum));
        }
    }

    /// <summary>
    /// Spawns a pooled enemy.
    /// </summary>
    /// <param name="enemy">Enemy to spawn.</param>
    /// <param name="position">Enemy's position</param>
    /// <param name="rotation">Enemy's rotation</param>
    /// <returns>Spawned enemy instance</returns>
    public Enemy SpawnEnemy(Enemy enemy, Vector3 position, Quaternion rotation) {

        // Try to find requested enemy
        // THIS MUST ALSO IMPROVE!!!!!
        Enemy e = inactiveEnemies.Find(x => x.name == enemy.name + "(Clone)");

        if (e != null) {

            e.transform.position = position;
            e.transform.rotation = rotation;
            e.gameObject.SetActive(true);

            inactiveEnemies.Remove(e);
            activeEnemies.Add(e);

            return e;
        }

        Debug.LogError("EnemyManager::spawnEnemy() => Could not find requested enemy in inactiveEnemies.");

        return null;
    }

    /// <summary>
    /// Despawns an enemy that HAS to be pooled;
    /// </summary>
    /// <param name="enemy">Enemy to despawn</param>
    /// <returns>Despawned enemy instance</returns>
    public Enemy DespawnEnemy(Enemy enemy) {

        enemy.gameObject.SetActive(false);
        enemy.transform.position = inactiveCoords;
        enemy.transform.rotation = Quaternion.identity;

        activeEnemies.Remove(enemy);
        inactiveEnemies.Add(enemy);

        return enemy;
    }

    [System.Serializable]
    public struct PooledEnemy {

        public int NumToSpawn;

        public Enemy Enemy;
    }
}
