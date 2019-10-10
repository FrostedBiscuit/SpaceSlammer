using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    List<Skin> SkinConfig = new List<Skin>();

    [SerializeField]
    Button UnlockButton = null;

    List<Skin> skins = new List<Skin>();

    int currentSkinIndex;

    SpriteRenderer playerSR;

    // Start is called before the first frame update
    void Start() {

        skins = SkinConfig;

        playerSR = Player.instance.GetComponentInChildren<SpriteRenderer>();

        for (int i = 0; i < skins.Count; i++) {
            skins[i].Init();
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

        currentSkinIndex = (currentSkinIndex + 1) % skins.Count;

        UnlockButton.interactable = ScoreManager.instance.GetHighScore() >= skins[currentSkinIndex].Cost;

        updatePlayerSprite();
    }

    public void DecreaseSkinindex() {

        currentSkinIndex = currentSkinIndex - 1 == -1 ? skins.Count - 1 : currentSkinIndex - 1;

        UnlockButton.interactable = ScoreManager.instance.GetHighScore() >= skins[currentSkinIndex].Cost;

        updatePlayerSprite();
    }

    public void UnlockSelected() {

        if (skins[currentSkinIndex].GetUnlockState()) {
            return;
        }

        // Some kind of score validation...
        if (ScoreManager.instance.GetHighScore() >= skins[currentSkinIndex].Cost) {

            skins[currentSkinIndex].Unlock();

            updatePlayerSprite();
        }
    }

    private void updatePlayerSprite() {

        if (skins[currentSkinIndex].GetUnlockState() == true) {

            playerSR.sprite = skins[currentSkinIndex].SkinSprite;

            PlayerPrefs.SetInt("SelectedSkinIndex", currentSkinIndex);

            Debug.Log("Updating unlocked skin.");
        }

        CurrentSkin = skins[currentSkinIndex];
    }

    [System.Serializable]
    public class Skin {

        public Sprite SkinSprite;

        public int Cost;

        public string Name;

        private bool unlocked;

        [SerializeField]
        bool LockStatus = false;

        public void Init() {

            if (PlayerPrefs.HasKey(SkinSprite.name + "LockState")) {

                unlocked = PlayerPrefs.GetInt(SkinSprite.name + "LockState") == 1;
            }
            else { 

                PlayerPrefs.SetInt(SkinSprite.name + "LockState", LockStatus ? 1 : 0);

                unlocked = LockStatus;
            }
        }

        public void Unlock() {

            unlocked = true;

            PlayerPrefs.SetInt(SkinSprite.name + "LockState", 1);
        }

        public bool GetUnlockState() {
            return unlocked;
        }
    }
}
