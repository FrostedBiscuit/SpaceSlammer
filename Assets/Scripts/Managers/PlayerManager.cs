using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField]
    GameObject PlayerGO = null;

    [SerializeField]
    List<Sprite> PlayerSkins = new List<Sprite>();

    [SerializeField]
    Transform Spawn = null;

    [SerializeField]
    CameraFollow cameraFollow = null;
#if UNITY_EDITOR
    [SerializeField]
    int DEBUG_TestPlayerSkinIndex = 0;
#endif
    int currentIndex {
        get {
            return _currIndex;
        }
        set {
            _currIndex = value;

            updatePlayerSprite();
        }
    }

    int _currIndex = 0;

    SpriteRenderer playerSpriteRenderer = null;

    // Start is called before the first frame update
    void Start() {
#if UNITY_EDITOR
        PlayerPrefs.SetInt("CurrentPlayerSkinIndex", DEBUG_TestPlayerSkinIndex);
        currentIndex = DEBUG_TestPlayerSkinIndex;
#else
        if (PlayerPrefs.HasKey("CurrentPlayerSkinIndex") == true) {
            currentIndex = PlayerPrefs.GetInt("CurrentPlayerSkinIndex");
        }
        else {
            PlayerPrefs.SetInt("CurrentPlayerSkinIndex", 0);
        }
#endif
        if (Player.instance != null) {
            playerSpriteRenderer = Player.instance.transform.GetComponentInChildren<SpriteRenderer>();
        }
        else {
            SpawnPlayer();
        }

        updatePlayerSprite();
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
            return;
        }

        GameObject go = ObjectPool.instance.RequestObject(PlayerGO, Spawn.position, Spawn.rotation);

        playerSpriteRenderer = Player.instance.transform.GetComponentInChildren<SpriteRenderer>();
        playerSpriteRenderer.sprite = PlayerSkins[currentIndex];

        cameraFollow.FollowTransform = go.transform;

        PlayerPrefs.SetInt("CurrentPlayerSkinIndex", currentIndex);

        updatePlayerSprite();
    }

    public void IncreasePlayerSkinIndex() {
        currentIndex = (currentIndex + 1) % PlayerSkins.Count;
    }

    public void DecreasePlayerSkinIndex() {

        currentIndex = (currentIndex - 1) < 0 ? PlayerSkins.Count - 1 : currentIndex - 1;
    }

    void updatePlayerSprite() {

        if (playerSpriteRenderer != null) {
            playerSpriteRenderer.sprite = PlayerSkins[currentIndex];
        }
        else {
            Debug.LogWarning("PlayerManager::updatePlayerSprite() => No playerSpriteRenderer found!");
        }
    }
}
