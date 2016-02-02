using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]

public class PlaySoundDevice : Device {

    public bool playOnce,dontStop;
    AudioSource soundToPlay;
    float baseVolume,lerpSoundToThis;
    bool played;
	void Start () {
        soundToPlay = GetComponent<AudioSource>();
        baseVolume = soundToPlay.volume;
    }
	
	void Update () {
        soundToPlay.volume = Mathf.MoveTowards(soundToPlay.volume, lerpSoundToThis, 2 * Time.deltaTime);
	}

    public override void Activate()
    {
        if (!soundToPlay.isPlaying && !played)
        {
            if (playOnce)
            {
                played = true;
            }
            soundToPlay.Play();
        }

        lerpSoundToThis = baseVolume;
        activated = true;
    }

    public override void Deactivate()
    {
        if (!dontStop)
        {
            activated = false;
            lerpSoundToThis = 0;
        }
    }

}