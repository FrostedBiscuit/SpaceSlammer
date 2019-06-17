using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleScoreButton : MonoBehaviour {

    [SerializeField]
    float OpenSpeed = 10f;

    [SerializeField]
    bool IsOn = false;

    [SerializeField]
    RectTransform TextBackgroundRect = null;
    [SerializeField]
    RectTransform TextRect = null;

    [SerializeField]
    CanvasGroup TextCanvasGroup = null;

    Vector2 closedPos;
    Vector2 openPos;

    void Start() {
        
        if (TextBackgroundRect == null) {

            Debug.LogError("UIToggleScoreButton::Start() => No text background RectTransform found!!!");

            return;
        }

        if (TextRect == null) {

            Debug.LogError("UIToggleScoreButton::Start() => No text RectTransform found!!!");

            return;
        }

        if (TextCanvasGroup == null) {

            Debug.LogError("UIToggleScoreButton::Start() => No text CanvasGroup found!!!");

            return;
        }

        closedPos = new Vector2(0f, TextBackgroundRect.sizeDelta.y);

        TextBackgroundRect.sizeDelta = closedPos;

        TextCanvasGroup.alpha = 0f;
    }

    // Update is called once per frame
    void Update() {

        openPos.x = TextRect.sizeDelta.x;

        if (IsOn == true) {

            TextBackgroundRect.sizeDelta = new Vector2(Mathf.Lerp(TextBackgroundRect.sizeDelta.x, openPos.x, OpenSpeed * Time.deltaTime), TextBackgroundRect.sizeDelta.y);

            if (openPos.x - TextBackgroundRect.sizeDelta.x <= 5f) {
                TextCanvasGroup.alpha = 1f;
            }
        } 
        else {

            TextBackgroundRect.sizeDelta = new Vector2(Mathf.Lerp(TextBackgroundRect.sizeDelta.x, closedPos.x, OpenSpeed * Time.deltaTime), TextBackgroundRect.sizeDelta.y);

            TextCanvasGroup.alpha = 0f;
        }
    }

    public void Toggle() {

        IsOn = !IsOn;
    }
}
