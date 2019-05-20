using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField]
    GameObject PlayerGO;

    [SerializeField]
    Transform Spawn;

    [SerializeField]
    CameraFollow cameraFollow;

    // Start is called before the first frame update
    void Start() {
        
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

        if (Player.instance == null) {

            GameObject go = ObjectPool.instance.RequestObject(PlayerGO, Spawn.position, Spawn.rotation);

            cameraFollow.FollowTransform = go.transform;
        }
    }
}
