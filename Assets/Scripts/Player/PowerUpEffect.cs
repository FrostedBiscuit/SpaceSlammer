using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour {

    [System.Serializable]
    public class PowerUpVFX {
        public PlayerManager.Effect EffectType;
        public Sprite EffectSprite;

        [HideInInspector]
        public bool IsRunning;

        [HideInInspector]
        public Coroutine VFX;
    }

    SpriteRenderer sr;

    Color initialSRColor;

    [SerializeField]
    PowerUpVFX[] VFX;

    public void Enable(PlayerManager.Effect effect, float duration) {
        
        VFX.First(e => e.EffectType == effect).VFX = StartCoroutine(effectVFX(effect, duration));
    }

    IEnumerator effectVFX(PlayerManager.Effect effect, float duration) {

        if (VFX.First(e => e.EffectType == effect).IsRunning) {
            StopCoroutine(VFX.First(e => e.EffectType == effect).VFX);
        }

        VFX.First(e => e.EffectType == effect).IsRunning = true;

        float ending = duration * 0.2f;

        sr.sprite = VFX.First(e => e.EffectType == effect).EffectSprite;

        yield return new WaitForSeconds(duration - ending);

        for (int i = 0; i < 4; i++) {

            Color currColor = sr.color;

            currColor.a = currColor.a == 1f ? 0f : 1f;

            sr.color = currColor;

            yield return new WaitForSeconds(ending / 4f);
        }

        sr.sprite = null;
        sr.color = initialSRColor;

        VFX.First(e => e.EffectType == effect).IsRunning = false;
        VFX.First(e => e.EffectType == effect).VFX = null;
    }

    private void OnEnable() {

        sr = sr == null ? GetComponent<SpriteRenderer>() : sr;

        initialSRColor = sr.color;
    }

    private void OnDisable() {

        StopAllCoroutines();

        sr.sprite = null;
        sr.color = initialSRColor;
    }
}
