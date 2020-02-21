using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : MonoBehaviour {

    [SerializeField]
    float MainMenuDefaultZoom = 4f;

    [SerializeField]
    CameraZoom CameraZoom = null;

    [SerializeField]
    AudioSource MainCameraAudioSource = null;

    public void Pause() {

        Time.timeScale = 0f;
    }

    public void Unpause() {

        Time.timeScale = 1f;
    }

    public void Back() {

        PlayerManager.instance.DespawnPlayer();

        EnemyManager.instance.EndSpawning();
        EnemyManager.instance.ClearEnemies();

        ConsumablesManager.instance.StopSpawningConsumables();
        ConsumablesManager.instance.ClearConsumables();

        UIManager.instance.ActivateMainMenu();

        PlayerManager.instance.SpawnPlayer();

        MainCameraAudioSource.mute = false;

        CameraZoom.SetDefaultZoom(MainMenuDefaultZoom);

        Unpause();

        gameObject.SetActive(false);
    }
}
