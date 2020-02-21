using System.Linq;
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
    CanvasGroup MainMenu = null;

    [SerializeField]
    GameObject HUD = null;
    [SerializeField]
    GameObject EndScreen = null;

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

            MainMenu.interactable = true;
            MainMenu.blocksRaycasts = true;
            MainMenu.alpha = 1f;

            HUD.SetActive(false);
            EndScreen.SetActive(false);
        }
    }

    public void ActivateHUD() {

        if (HUD != null && MainMenu != null && EndScreen != null) {

            HUD.SetActive(true);

            EndScreen.SetActive(false);

            Debug.Log("Deactivating main menu");

            MainMenu.interactable = false;
            MainMenu.blocksRaycasts = false;
            MainMenu.alpha = 0f;
        }
    }

    public void ActivateEndScreen() {

        if (EndScreen != null && HUD != null && MainMenu != null) {

            EndScreen.SetActive(true);

            HUD.SetActive(false);

            MainMenu.interactable = false;
            MainMenu.blocksRaycasts = false;
            MainMenu.alpha = 0f;
        }
    }
}
