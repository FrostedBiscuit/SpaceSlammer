using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public GameObject HealthBarObject;

    public Image HealthBar;

    public TextMeshProUGUI BossNameLabel;

    public void Enable(string name) {

        HealthBarObject.SetActive(true);

        HealthBar.fillAmount = 1f;

        BossNameLabel.text = name.ToUpper();
    }

    public void Disable() {

        HealthBarObject.SetActive(false);

        BossNameLabel.text = "";
    }

    public void UpdateHealth(float health, float maxHealth) {

        HealthBar.fillAmount = health / maxHealth;
    }
}
