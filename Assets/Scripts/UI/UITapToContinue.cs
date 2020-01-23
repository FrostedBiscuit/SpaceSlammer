using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITapToContinue : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    float DisableAfter = 0f;

    [SerializeField]
    Animator PlayerAnimator = null;
    [SerializeField]
    Animator CanvasAnimator = null;

    [SerializeField]
    CameraFollow CameraFollow = null;
    
    public void OnPointerClick(PointerEventData eventData) {

        PlayerAnimator.SetTrigger("PlayerSlideIn");
        CanvasAnimator.SetTrigger("Finish");

        Invoke("disableAnimEnableCamFollow", DisableAfter);
    }

    private void disableAnimEnableCamFollow() {

        if (PlayerAnimator != null) {

            PlayerAnimator.enabled = false;
        }

        if (CameraFollow != null) {

            CameraFollow.enabled = true;
        }
    }
}
