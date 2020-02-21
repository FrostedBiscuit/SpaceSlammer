using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISkinMenu : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI SkinNameTag = null;
    [SerializeField]
    TextMeshProUGUI SkinPriceTag = null;

    [SerializeField]
    GameObject Lock = null;

    void Start() {
        
        if (SkinNameTag == null) {
            Debug.LogError("UISkinMenu::Start() => No skin name tag found!!!");
        }
        if (SkinPriceTag == null) {
            Debug.LogError("UISkinMenu::Start() => No skin price tag found!!!");
        }

        if (Lock == null) {
            Debug.LogError("UISkinMenu::Start() => No Lock GO found!!!");
        }

        UpdateSkinMenu();
    }

    public void UpdateSkinMenu() {

        if (SkinManager.instance.CurrentSkin.GetUnlockState() == false) {
            Lock.SetActive(true);
        }
        else {
            Lock.SetActive(false);
        }

        SkinNameTag.text = SkinManager.instance.CurrentSkin.Name;
        SkinPriceTag.text = SkinManager.instance.CurrentSkin.Cost.ToString();
    }
}
