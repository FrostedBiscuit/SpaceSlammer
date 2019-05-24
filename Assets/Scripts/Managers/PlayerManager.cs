using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField]
    List<GameObject> PlayerGOs = new List<GameObject>();

    [SerializeField]
    Transform Spawn;

    [SerializeField]
    CameraFollow cameraFollow;
#if UNITY_EDITOR
    [SerializeField]
    int DEBUG_TestPlayerGOIndex = 0;
#endif
    int currentIndex {
        get {
            return _currIndex;
        }
        set {
            _currIndex = value;

            SpawnPlayer();
        }
    }

    int _currIndex = 0;

    // Start is called before the first frame update
    void Start() {
#if UNITY_EDITOR
        if (PlayerPrefs.HasKey("CurrentPlayerGOIndex") == true && PlayerPrefs.GetInt("CurrentPlayerGOIndex") == DEBUG_TestPlayerGOIndex) {
            currentIndex = PlayerPrefs.GetInt("CurrentPlayerGOIndex");
        }
        else {
            PlayerPrefs.SetInt("CurrentPlayerGOIndex", DEBUG_TestPlayerGOIndex);
            currentIndex = DEBUG_TestPlayerGOIndex;
        }

        if (Player.instance == null || PlayerGOs.IndexOf(Player.instance.gameObject) != DEBUG_TestPlayerGOIndex) {

            SpawnPlayer();
        }
#else
        if (PlayerPrefs.HasKey("CurrentPlayerGOIndex") == true) {
            currentIndex = PlayerPrefs.GetInt("CurrentPlayerGOIndex");
        }
        else {
            PlayerPrefs.SetInt("CurrentPlayerGOIndex", 0);
        }

        if (Player.instance == null) {
            SpawnPlayer();
        }
#endif
    }

    // Update is called once per frame
    void Update() {
        
        if (Player.instance == null) {
            // Player has died, do something

            EnemyManager.instance.EndSpawning();

            UIManager.instance.ActivateEndScreen();
        }
    }

    public void SpawnPlayer() {

        if (Player.instance != null) {
            
            ObjectPool.instance.ReturnObject(Player.instance.gameObject);
        }

        GameObject go = ObjectPool.instance.RequestObject(PlayerGOs[currentIndex], Spawn.position, Spawn.rotation);

        cameraFollow.FollowTransform = go.transform;

        PlayerPrefs.SetInt("CurrentPlayerGOIndex", currentIndex);
    }

    public void IncreasePlayerGOIndex() {
        currentIndex = (currentIndex + 1) % PlayerGOs.Count;
    }

    public void DecreasePlayerGOIndex() {

        currentIndex = (currentIndex - 1) < 0 ? PlayerGOs.Count - 1 : currentIndex - 1;
    }
}
