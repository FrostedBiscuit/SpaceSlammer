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
    int IndicatorsToSpawn = 10;

    [SerializeField]
    GameObject MainMenu = null;
    [SerializeField]
    GameObject HUD = null;
    [SerializeField]
    GameObject EndScreen = null;
    [SerializeField]
    GameObject EnemyIndicatorPrefab = null;

    [SerializeField]
    Transform EnemyIndicatorsParent= null;

    List<UIEnemyIndicator> activeIndicators = new List<UIEnemyIndicator>();
    List<UIEnemyIndicator> inactiveIndicators = new List<UIEnemyIndicator>();

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

        for (int i = 0; i < IndicatorsToSpawn; i++) {

            UIEnemyIndicator indicator = Instantiate(EnemyIndicatorPrefab, EnemyIndicatorsParent).GetComponent<UIEnemyIndicator>();

            indicator.gameObject.SetActive(false);

            inactiveIndicators.Add(indicator);
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

    public UIEnemyIndicator RequestEnemyIndicator() {

        UIEnemyIndicator indicator = inactiveIndicators[0];

        indicator.gameObject.SetActive(true);

        inactiveIndicators.Remove(indicator);
        activeIndicators.Add(indicator);

        return indicator;
    }

    public UIEnemyIndicator ReturnEnemyIndicator(UIEnemyIndicator indicator) {

        indicator.gameObject.SetActive(false);

        activeIndicators.Remove(indicator);
        inactiveIndicators.Add(indicator);

        return indicator;
    }
}
