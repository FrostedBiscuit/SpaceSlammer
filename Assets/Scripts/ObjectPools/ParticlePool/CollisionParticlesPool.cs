using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticlesPool : ObjectPool<Particles> {

    #region Singelton
    public static CollisionParticlesPool instance = null;

    private void Awake() {

        if (instance != null) {

            Debug.LogError($"{this.name}::Awake() => More than 1 instance of {this.name} in the scene!!");

            return;
        }

        instance = this;
    }
    #endregion
}
