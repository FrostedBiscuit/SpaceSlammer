using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHealthBar : MonoBehaviour {

    #region Singelton
    public static UIBossHealthBar instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("UIBossHealthBar::Awake() => More than 1 instance of UIBossHealthBar in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion

    public Image HealthBar;
    public Image HealthBarBG;

    public void Enable() {

        HealthBar.gameObject.SetActive(true);
        HealthBarBG.gameObject.SetActive(true);

        HealthBar.fillAmount = 1f;
    }

    public void Disable() {

        HealthBar.gameObject.SetActive(false);
        HealthBarBG.gameObject.SetActive(false);
    }

    public void UpdateHealth(float health, float maxHealth) {

        HealthBar.fillAmount = health / maxHealth;
    }
}
