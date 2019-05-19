using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    #region Singelton
    public static UIManager instance;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("UIManager::Awake() => More than 1 instance of UIManager in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject HUD;
    [SerializeField]
    GameObject EndScreen;

    // Start is called before the first frame update
    void Start() {
        
        if (MainMenu == null) {
            Debug.LogError("UIManager::Start() => MainMenu GameObject not assigned!!!");
        }

        if (HUD == null) {
            Debug.LogError("UIManager::Start() => HUD GameObject not assigned!!!");
        }

        if (EndScreen == null) {
            Debug.LogError("UIManager::Start() => EndScreen GameObject not assigned!!!");
        }
    }

    public void ActivateMainMenu() {

        if (MainMenu != null && HUD != null && EndScreen != null) {

            MainMenu.SetActive(true);

            HUD.SetActive(false);
            EndScreen.SetActive(false);
        }
    }

    public void ActivateHUD() {

        if (HUD != null && MainMenu != null && EndScreen != null) {

            HUD.SetActive(true);

            EndScreen.SetActive(false);
            MainMenu.SetActive(false);
        }
    }

    public void ActivateEndScreen() {

        if (EndScreen != null && HUD != null && MainMenu != null) {

            EndScreen.SetActive(true);

            HUD.SetActive(false);
            MainMenu.SetActive(false);
        }
    }
}
