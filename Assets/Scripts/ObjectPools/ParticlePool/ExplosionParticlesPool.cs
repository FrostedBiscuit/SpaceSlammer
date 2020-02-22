using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticlesPool : ObjectPool<ExplosionParticles> {

    #region Singelton
    public static ExplosionParticlesPool instance = null;

    private void Awake() {
        
        if (instance != null) {

            Debug.LogError("ExplosionParticlesPool::Awake() => More than 1 instance of ExplosionParticlesPool in the scene!!!");

            return;
        }

        instance = this;
    }
    #endregion
}
