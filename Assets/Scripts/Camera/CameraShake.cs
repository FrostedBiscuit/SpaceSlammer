using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    #region Singelton
    public static CameraShake instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("CameraShake::Awake() => More than 1 instance of CameraShake in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField]
    Animator Animator;

    public void Shake()
    {
        Animator.SetTrigger("Shake");
    }
}
