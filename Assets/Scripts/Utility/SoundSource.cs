using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour {

    bool initialized = false;

    AudioSource source = null;

    private void OnEnable() {

        source = source == null ? GetComponent<AudioSource>() : source;
    }

    private void Update() {
        
        if (initialized == true && source.isPlaying == false) {

            SoundSourcePool.instance.ReturnObject(this);
        }
    }

    private void OnDisable() {
        initialized = false;
    }

    public void Play(AudioClip clip) {

        source.PlayOneShot(clip);

        initialized = true;
    }
}
