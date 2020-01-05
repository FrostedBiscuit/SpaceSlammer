using System.Linq;
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

        foreach (EnemyPool ep in EnemyObjectPools) {

            ep.Init();
        }

        //Debug.Log(EnemyObjectPools.Count);

        /*for (int i = 0; i < EnemyObjectPools.Count; i++) {
            Debug.Log(EnemyObjectPools[i].name);
        }*/
    }
    #endregion

    [System.Serializable]
    public class EnemyPool {

        public GameObject PoolGO;

        [Range(0f, 1f)]
        public float SpawnProbability;

        ObjectPool<Enemy> pool;

        public void Init() {

            if (PoolGO == null) {

                Debug.LogError("EnemyPool has no PoolGO!!!");

                return;
            }

            pool = PoolGO.GetComponent<ObjectPool<Enemy>>();
        }

        public Enemy RequestObject(Vector3 position, Quaternion rotation) {
            return pool.RequestObject(position, rotation);
        }

        public Enemy ReturnObject(Enemy e) {
            return pool.ReturnObject(e);
        } 
    }

    public float EnemySpawnNearDist = 5f;
    public float EnemySpawnFarDist = 10f;
    public float MineCheckInterval = 5f;
    public float DifficultyFactor = 60f;

    public int MaxMines = 5;

    public List<EnemyPool> EnemyObjectPools = new List<EnemyPool>();

    public GameObject[] TestBosses = null;

    bool canSpawn = false;

    float sessionTime = 0f;

    int currentMines;
    int waves;

    List<Enemy> activeEnemies = new List<Enemy>();
    List<Mine> activeMines = new List<Mine>();

    private void Update() {

        sessionTime += Time.deltaTime;
    }

    public void StartSpawning() {

        if (canSpawn) {
            return;
        }

        ClearEnemies();

        canSpawn = true;

        sessionTime = 0f;

        waves = 0;

        spawnNewWave();

        InvokeRepeating("spawnMines", 0f, MineCheckInterval);

        ScoreManager.instance.ClearCurrentScore();

        Debug.Log($"Session time: {sessionTime}");
    }

    public void EndSpawning() {

        if (!canSpawn) {
            return;
        }

        CancelInvoke("spawnMines");

        canSpawn = false;

        ScoreManager.instance.CalculateScore(sessionTime);
    }

    public void ClearEnemies() {

        if (activeEnemies.Count == 0) return;

        for (int i = 0; i < activeEnemies.Count; i++) {

            activeEnemies[i].Dispose();
        }

        activeEnemies.Clear();

        clearMines();
    }

    private void spawnMines() {
        
        int mineDelta = MaxMines - currentMines;
        int numMinesToSpawn = Random.Range(0, mineDelta);

        for (int i = 0; i < numMinesToSpawn; i++) {

            float randomAngle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 newPos = Player.instance.transform.position +
                             new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(EnemySpawnNearDist, EnemySpawnFarDist);

            Mine m = MinePool.instance.RequestObject(newPos, Quaternion.identity);
            m.RegisterOnExplosionCallback(onMineExplosion);

            activeMines.Add(m);

            currentMines++;
        }
    }

    private void clearMines() {

        for (int i = 0; i < activeMines.Count; i++) {

            MinePool.instance.ReturnObject(activeMines[i]);
        }
    }

    // TEST CODE FOR GATHERING IDEAS!!!!
    // PRONE TO MUCH CHANGE
    private void spawnNewWave() {

        //Debug.Log("spawnNumEnemies");

        waves++;

        if (waves % 3 == 0) {

            Debug.Log($"Boss wave #{waves}");

            int randomIndex = Random.Range(0, TestBosses.Length);

            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(EnemySpawnNearDist, EnemySpawnFarDist);

            Enemy e = Instantiate(TestBosses[randomIndex], Player.instance.transform.position + offset, Quaternion.identity).GetComponent<Enemy>();
            e.RegisterOnDeathCallback(onEnemyDeath);

            activeEnemies.Add(e);

            return;
        }

        int enemyCount = Mathf.RoundToInt(1 + Mathf.Sqrt(sessionTime / DifficultyFactor));

        Debug.Log($"Enemies to spawn: {enemyCount}");

        for (int i = 0; i < enemyCount; i++) {

            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(EnemySpawnNearDist, EnemySpawnFarDist);

            float randomProbability = Random.Range(0f, 1f);
            EnemyPool randomPool = EnemyObjectPools.OrderBy(p => Mathf.Abs(randomProbability - p.SpawnProbability)).First();

            Enemy e = randomPool.RequestObject(Player.instance.transform.position + offset, Quaternion.identity);
            e.RegisterOnDeathCallback(onEnemyDeath);

            activeEnemies.Add(e);
        }
    }

    // TEST CODE FOR GATHERING IDEAS!!!!
    // PRONE TO MUCH CHANGE
    void onEnemyDeath(Enemy e) {

        e.UnregisterOnDeathCallback(onEnemyDeath);

        activeEnemies.Remove(e);

        ScoreManager.instance.UpdateCurrentScore(e.ScoreValue);

        if (activeEnemies.Count == 0 && canSpawn == true) {

            activeEnemies.Clear();

            spawnNewWave();
        }
    }

    void onMineExplosion(Mine m) {

        m.UnregisterOnExplosionCallback(onMineExplosion);

        currentMines--;

        if (currentMines < 0) {
            Debug.LogError($"EnemyManager::onMineExplosion() => currentMines is less than 0!!! currentMines: {currentMines}");
        }
    }
}
