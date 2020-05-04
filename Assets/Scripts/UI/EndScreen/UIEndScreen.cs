using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEndScreen : MonoBehaviour {

    [SerializeField]
    float MainMenuDefaultZoom = 4f;

    [SerializeField]
    CameraZoom CameraZoom = null;

    public void RestartGame() {

        DisposableManager.instance.DisposeAll();

        PlayerManager.instance.SpawnPlayer();

        UIManager.instance.ActivateHUD();

        EnemyManager.instance.ClearEnemies();
        EnemyManager.instance.StartSpawning();

        ConsumablesManager.instance.ClearConsumables();
        ConsumablesManager.instance.StartSpawningConsumables();
    }

    public void Exit() {

        ConsumablesManager.instance.StopSpawningConsumables();
        ConsumablesManager.instance.ClearConsumables();

        EnemyManager.instance.EndSpawning();
        EnemyManager.instance.ClearEnemies();

        DisposableManager.instance.DisposeAll();

        PlayerManager.instance.SpawnPlayer();

        UIManager.instance.ActivateMainMenu();

        CameraZoom.SetDefaultZoom(MainMenuDefaultZoom);
    }
}
