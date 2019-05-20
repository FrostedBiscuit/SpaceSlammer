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

    public int sumEnemyDied;

    public GameObject Enemy;

    List<Enemy> activeEnemies = new List<Enemy>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool canSpawn = false;

    public void StartSpawning() {

        ClearEnemies();

        canSpawn = true;

        sumEnemyDied = 0;

        spawnNumEnemies(Random.Range(1, 4));
    }

    public void EndSpawning() {

        canSpawn = false;
    }

    public void ClearEnemies() {

        if (activeEnemies.Count == 0) return;

        foreach (var enemy in activeEnemies) {

            Destroy(enemy.gameObject);
        }

        activeEnemies.Clear();
    }

    // TEST CODE FOR GATHERING IDEAS!!!!
    // PRONE TO MUCH CHANGE
    private void spawnNumEnemies(int enemyCount) {

        for (int i = 0; i < enemyCount; i++) {

            float randomAngle = Random.Range(0f, 2f * Mathf.PI);

            Vector3 offset = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(EnemySpawnNearDist, EnemySpawnFarDist); 

            Enemy e = ObjectPool.instance.RequestObject(Enemy, offset + Player.instance.transform.position, Quaternion.identity).GetComponent<Enemy>();

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

            spawnNumEnemies(Random.Range(1, 4));
        }
    }
}
