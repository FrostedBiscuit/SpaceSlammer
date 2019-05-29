using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    #region Singelton
    public static SoundManager instance;

    private void Awake() {

        if (instance != null) {

            Debug.LogError("SoundManager::Awake() => More than 1 instance of SoundManager in the scene!!!");
            return;
        }

        instance = this;
    }
    #endregion

    bool playSFX = false;
    bool playMusic = false;

    public bool PlaySFX { get { return playSFX; } }
    public bool PlayMusic { get { return playMusic; } }

    [SerializeField]
    AudioSource Source = null;

    [SerializeField]
    List<AudioClip> MusicTracks = new List<AudioClip>();

    // Start is called before the first frame update
    void Start() {

        if (Source == null) {

            Debug.LogError("SoundManager::Start() => No Source found!!!");
        }

        if (MusicTracks.Count == 0) {

            Debug.LogWarning("SoundManager::Start() => No music tracks set. There will be no music playing.");
        }
    }

    int trackNumber = 0;

    // Update is called once per frame
    void Update() {

        // if we want music
        if (playMusic == true && MusicTracks.Count != 0) {

            if (Source.isPlaying == false) {

                Source.PlayOneShot(MusicTracks[trackNumber]);

                trackNumber = (trackNumber + 1) % MusicTracks.Count;
            }
        }
        else {

            Source.Stop();
        }
    }

    public void SetPlaySFX(bool value) { playSFX = value; }

    public void SetPlayMusic(bool value) { playMusic = value; }

    public void PlaySFXClip(AudioClip clip) {

        if (playSFX == false) return;

        Source.PlayOneShot(clip);
    }
}
