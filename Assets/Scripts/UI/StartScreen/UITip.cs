using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITip : MonoBehaviour {

    [System.Serializable]
    public class Tip {
        [TextArea]
        public string TipText;

        public Sprite TipSprite;
    }

    [SerializeField]
    TextMeshProUGUI TipText = null;

    [SerializeField]
    Image TipImage = null;

    [SerializeField]
    Tip[] Tips = null;

    private void OnEnable() {

        if (TipImage == null) {
            Debug.LogError("UITip::OnEnable() => No TipImage found!!!");
        }

        if (TipText == null) {
            Debug.LogError("UITip::OnEnable() => No TipText found!!!");
        }

        if (Tips.Length == 0) {

            Debug.LogWarning("UITip::OnEnable() => No tips set...none will be displayed.");

            return;
        }

        int randomTipIndex = Random.Range(0, Tips.Length);

        TipText.text = $"TIP:\n{Tips[randomTipIndex].TipText.ToUpper()}";
        TipImage.sprite = Tips[randomTipIndex].TipSprite;
    }
}
