using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayButton : MonoBehaviour {

    [SerializeField]
    float DefaultZoom = 8f;

    [SerializeField]
    CameraZoom CameraZoom = null;

    public void StartGame() {

        EnemyManager.instance.StartSpawning();

        ConsumablesManager.instance.StartSpawningConsumables();
        
        UIManager.instance.ActivateHUD();

        CameraZoom.SetDefaultZoom(DefaultZoom);
    }
}
