using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * HOW TO USE:
 * To play a SFX (eg Button1 clip) in your other script, you can call
 * 
 * AudioManager.Instance.PlaySfx("Button1"); // or
 *
 * AudioManager.Instance.PlaySfx("Button1", 1.5f) 
 *
 * 
 *
 * 
 */
public class AudioManager : Singleton<AudioManager>
{
	//[SerializeField] 
	AudioSource musicSource;
    //[SerializeField] 
    AudioSource sfxSource;

    public void PlayMusic(string clipName, bool loop=true) {
		musicSource.clip = Resources.Load<AudioClip>("Sound/"+clipName);
        musicSource.volume = GameManager.Instance.GameSetting.MusicVolume;
        musicSource.loop = loop;
		musicSource.Play();
    }
    public void PlayMusic(string clipName) {
	    musicSource.clip = Resources.Load<AudioClip>("Sound/"+clipName);
	    musicSource.volume = GameManager.Instance.GameSetting.MusicVolume;
	    musicSource.loop = true;
	    musicSource.Play();
    }
    
	public void PlaySfx(string clipName, float pitch=1.0f) {
		sfxSource.clip =  Resources.Load<AudioClip>("Sound/"+clipName);
        sfxSource.volume = GameManager.Instance.GameSetting.SfxVolume;
        sfxSource.pitch = pitch;
		sfxSource.Play();
	}
	public void PlaySfx(string clipName) {
		sfxSource.clip =  Resources.Load<AudioClip>("Sound/"+clipName);
		sfxSource.volume = GameManager.Instance.GameSetting.SfxVolume;
		sfxSource.pitch = 1.0f;
		sfxSource.Play();
	}

	public void StopMusic() {
		musicSource.Stop();
	}

	private void SetMusicVolume(float value) {
        musicSource.volume = value;
    }
    private void SetSfxVolume(float value) {
        sfxSource.volume = value;
    }

    private void Start() {
	    musicSource = transform.AddComponent<AudioSource>();
	    sfxSource = transform.AddComponent<AudioSource>();
	    
	    AudioManager.Instance.PlayMusic("MainTheme");
    }

    void OnEnable() {
        GameManager.Instance.GameSetting.musicVolumeUpdated += SetMusicVolume;
        GameManager.Instance.GameSetting.sfxVolumeUpdated += SetSfxVolume;
    }
}