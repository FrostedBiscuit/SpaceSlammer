using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISkinMenu : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI SkinNameTag = null;
    [SerializeField]
    TextMeshProUGUI SkinPriceTag = null;

    // Start is called before the first frame update
    void Start() {
        
        if (SkinNameTag == null) {
            Debug.LogError("UISkinMenu::Start() => No skin name tag found!!!");
        }
        if (SkinPriceTag == null) {
            Debug.LogError("UISkinMenu::Start() => No skin price tag found!!!");
        }
    }

    // Update is called once per frame
    void Update() {

        SkinNameTag.text = SkinManager.instance.CurrentSkin.Name;
        SkinPriceTag.text = SkinManager.instance.CurrentSkin.Cost.ToString();
    }
}
