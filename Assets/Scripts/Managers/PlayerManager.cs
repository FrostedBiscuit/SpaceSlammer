using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

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

        PlayerGO.SetActive(false);
    }
}
