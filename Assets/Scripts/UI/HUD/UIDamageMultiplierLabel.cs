using UnityEngine;
using TMPro;

public class UIDamageMultiplierLabel : MonoBehaviour {

    TextMeshProUGUI label = null;

    // Start is called before the first frame update
    void Start() {

        label = GetComponent<TextMeshProUGUI>();
    }
    
    void Update() {
        
        if (Player.instance.gameObject.activeSelf == true) {

            label.text = $"DAMAGE MULTIPLIER: x{Player.instance.DamageMultiplier:F2}";
        }
    }
}
