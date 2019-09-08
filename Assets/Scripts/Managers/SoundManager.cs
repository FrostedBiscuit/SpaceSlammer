using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public bool PlaySFX {
        get {
            return _playSFX;
        }
        set {

            _playSFX = value;

            updateSFXToggles();
        }
    }

    bool _playSFX = false;

    public bool PlayMusic {
        get {
            return _playMusic;
        }
        set {

            _playMusic = value;

            updateMusicToggles();
        }
    }

    bool _playMusic = false;

    [SerializeField]
    AudioSource Source = null;

    [SerializeField]
    List<GameObject> SFXToggleSwitches = new List<GameObject>();
    [SerializeField]
    List<GameObject> MusicToggleSwitches = new List<GameObject>();

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
        if (PlayMusic == true && MusicTracks.Count != 0) {

            if (Source.isPlaying == false) {

                Source.PlayOneShot(MusicTracks[trackNumber]);

                trackNumber = (trackNumber + 1) % MusicTracks.Count;
            }
        }
        else {

            Source.Stop();
        }
    }

    void updateSFXToggles() {

        for (int i = 0; i < SFXToggleSwitches.Count; i++) {

            SFXToggleSwitches[i].GetComponent<Toggle>().isOn = PlaySFX;
        }
    }

    void updateMusicToggles() {

        for (int i = 0; i < MusicToggleSwitches.Count; i++) {

            MusicToggleSwitches[i].GetComponent<Toggle>().isOn = PlayMusic;
        }
    }

    public void SetPlaySFX(bool value) { PlaySFX = value; }

    public void SetPlayMusic(bool value) { PlayMusic = value; }

    public void PlayRemoteSFXClip(AudioClip clip, Vector3 position) {

        if (PlaySFX == false) {
            return;
        }

        SoundSource ss = SoundSourcePool.instance.RequestObject(position, Quaternion.identity);
        ss.Play(clip);
    }
}
