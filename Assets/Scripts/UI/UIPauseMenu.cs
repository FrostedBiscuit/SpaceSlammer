using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour {

    [SerializeField]
    CanvasGroup CanvasGroup = null;

    public void Toggle() {

        if (CanvasGroup == null) {

            Debug.LogError("UIPauseMenu::Toggle() => No canvas group found!!!");

            return;
        }

        CanvasGroup.alpha = CanvasGroup.alpha == 0f ? 1f : 0f;
        CanvasGroup.blocksRaycasts = !CanvasGroup.blocksRaycasts;
        CanvasGroup.interactable = !CanvasGroup.interactable;
    }

    public void Pause() {

        Time.timeScale = 0f;
    }

    public void Unpause() {

        Time.timeScale = 1f;
    }
}
