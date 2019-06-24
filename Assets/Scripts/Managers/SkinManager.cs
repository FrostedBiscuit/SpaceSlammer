using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour {

    #region Singelton
    public static SkinManager instance;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("SkinManager::Awake() => More than 1 instance of SkinManager in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion

    public Skin CurrentSkin { get; protected set; }

    [SerializeField]
    List<Skin> Skins = new List<Skin>();
    
    int currentSkinIndex {

        get {
            return _currentSkinIndex;
        }
        set {

            updatePlayerSprite();

            _currentSkinIndex = value;
        }
    }

    int _currentSkinIndex = 0;

    SpriteRenderer playerSR;

    // Start is called before the first frame update
    void Start() {

        playerSR = Player.instance.GetComponentInChildren<SpriteRenderer>();

        for (int i = 0; i < Skins.Count; i++) {
            Skins[i].Init();
        }

        if (PlayerPrefs.HasKey("SelectedSkinIndex")) {

            currentSkinIndex = PlayerPrefs.GetInt("SelectedSkinIndex");
        }
        else {

            currentSkinIndex = 0;
                
            PlayerPrefs.SetInt("SelectedSkinIndex", 0);
        }

        updatePlayerSprite();
    }

    public void IncreaseSkinIndex() {

        currentSkinIndex = (currentSkinIndex + 1) % Skins.Count;
    }

    public void DecreaseSkinindex() {

        currentSkinIndex = currentSkinIndex - 1 == -1 ? Skins.Count - 1 : currentSkinIndex - 1;
    }

    public void UnlockSelected() {

        if (Skins[currentSkinIndex].GetUnlockState()) {
            return;
        }

        // Some kind of score validation...

        Skins[currentSkinIndex].Unlock();

        updatePlayerSprite();
    }

    private void updatePlayerSprite() {

        if (Skins[currentSkinIndex].GetUnlockState() == true) {

            playerSR.sprite = Skins[currentSkinIndex].SkinSprite;

            PlayerPrefs.SetInt("SelectedSkinIndex", currentSkinIndex);
        }

        CurrentSkin = Skins[currentSkinIndex];
    }

    [System.Serializable]
    public struct Skin {

        public Sprite SkinSprite;

        public int Cost;

        public string Name;

        [SerializeField]
        bool Unlocked;

        public void Init() {

            if (PlayerPrefs.HasKey(SkinSprite.name + "LockState")) {

                Unlocked = PlayerPrefs.GetInt(SkinSprite.name + "LockState") == 1;
            }
            else { 
                PlayerPrefs.SetInt(SkinSprite.name + "LockState", Unlocked ? 1 : 0);
            }
        }

        public void Unlock() {

            Unlocked = true;

            PlayerPrefs.SetInt(SkinSprite.name + "LockState", 1);
        }

        public bool GetUnlockState() {
            return Unlocked;
        }
    }
}
