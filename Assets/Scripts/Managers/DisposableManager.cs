using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposableManager : MonoBehaviour {

    #region Singelton
    public static DisposableManager instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("DisposableManager::Awake() => More than 1 instance of DisposableManager in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion

    public void DisposeAll() {

        List<IDisposable> disposables = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IDisposable>().ToList();

        foreach (var disposable in disposables) {

            disposable.Dispose();
        }
    }
}
