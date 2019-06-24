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
/*
    [SerializeField]
    List<Sprite> PlayerSkins = new List<Sprite>();
*/
    [SerializeField]
    Transform Spawn = null;
/*
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

            PlayerPrefs.SetInt("CurrentPlayerSkinIndex", currentIndex);

            updatePlayerSprite();
        }
    }

    int _currIndex = 0;

    //SpriteRenderer playerSpriteRenderer = null;
*/
    // Start is called before the first frame update
    void Start() {

        SpawnPlayer();

        //Debug.Log("spriteIndex " + currentIndex);
        /*
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
#endif*/
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

        //updatePlayerSprite();
    }

    public void DespawnPlayer() {

        if (Player.instance.gameObject.activeSelf == false) {
            return;
        }

        PlayerGO.SetActive(false);
    }
/*
    public void IncreasePlayerSkinIndex() {
        currentIndex = (currentIndex + 1) % PlayerSkins.Count;
    }

    public void DecreasePlayerSkinIndex() {

        currentIndex = (currentIndex - 1) < 0 ? PlayerSkins.Count - 1 : currentIndex - 1;
    }

    void updatePlayerSprite() {

        SpriteRenderer playerSpriteRenderer = PlayerGO.transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (playerSpriteRenderer != null) {
            playerSpriteRenderer.sprite = PlayerSkins[currentIndex];
        }
        else {
            Debug.LogWarning("PlayerManager::updatePlayerSprite() => No playerSpriteRenderer found!");
        }
    }*/
}
