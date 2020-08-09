using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {

    [SerializeField]
    HeavyWeaponsShip ParentShip = null;

    [SerializeField]
    AudioSource Source = null;

    [SerializeField]
    AudioClip[] BeamSounds = null;

    bool damagePlayer = false;

    float nextDamageTime;
    float lifetime;

    private void Update() 
    {
        if (lifetime < Time.time) 
        {
            gameObject.SetActive(false);

            return;
        }
        
        if (damagePlayer == true && nextDamageTime <= Time.time && Player.instance.gameObject.activeSelf == true) 
        {
            Player.instance.TakeDamage(ParentShip.AttackDamage * Random.Range(0f, 1f));

            nextDamageTime = Time.time + ParentShip.BeamDamageInterval;
        }

        if (SoundManager.instance.PlaySFX == true &&  BeamSounds.Length > 0 && Source.isPlaying == false) 
        {
            var randomBeamSoundIndex = Random.Range(0, BeamSounds.Length);

            Source.PlayOneShot(BeamSounds[randomBeamSoundIndex]);
        }
    }

    private void OnEnable() 
    {
        nextDamageTime = 0f;
        lifetime = Time.time + ParentShip.BeamDuration;
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.transform.tag == "Player")
        { 
            damagePlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.transform.tag == "Player")
        {
            damagePlayer = false;
        }
    }
}
