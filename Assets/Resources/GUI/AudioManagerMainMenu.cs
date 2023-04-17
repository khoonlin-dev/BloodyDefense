using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerMainMenu : MonoBehaviour {

    public AudioClip normalButtonAudio;

    public AudioClip returnButtonAudio;

    public AudioClip settingButtonAudio;

    public AudioClip mainMenuMusic;

    public AudioSource soundAudioPlayer;

    public AudioSource musicAudioPlayer;

    private string soundVolumeStringKey = "Sound Volume";

    private string musicVolumeStringKey = "Music Volume";

    private int soundVolume;

    private int musicVolume;



	// Use this for initialization
	void Start () {
        soundVolume = 10;
        musicVolume = 10;

        if (!PlayerPrefs.HasKey(musicVolumeStringKey))
        {
            PlayerPrefs.SetInt(musicVolumeStringKey, 50);
        }

        musicVolume = PlayerPrefs.GetInt(musicVolumeStringKey);

        if (mainMenuMusic != null)
        {
            musicAudioPlayer.clip = mainMenuMusic;

            musicAudioPlayer.volume = musicVolume / 100.0f;

            musicAudioPlayer.Play();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playNormalButtonSound()
    {
        if (soundAudioPlayer != null && soundAudioPlayer.isPlaying)
        {
            soundAudioPlayer.Stop();
        }

        if (normalButtonAudio != null)
        {
            soundAudioPlayer.clip = normalButtonAudio;

            if (PlayerPrefs.HasKey(soundVolumeStringKey))
            {
                soundVolume = PlayerPrefs.GetInt(soundVolumeStringKey);
            }

            soundAudioPlayer.volume = soundVolume / 100.0f;

            soundAudioPlayer.Play();
        }
    }

    public void playReturnButtonSound()
    {
        if (soundAudioPlayer != null && soundAudioPlayer.isPlaying)
        {
            soundAudioPlayer.Stop();
        }

        if (returnButtonAudio != null)
        {
            soundAudioPlayer.clip = returnButtonAudio;

            if (PlayerPrefs.HasKey(soundVolumeStringKey))
            {
                soundVolume = PlayerPrefs.GetInt(soundVolumeStringKey);
            }

            soundAudioPlayer.volume = soundVolume / 100.0f;

            soundAudioPlayer.Play();
        }
    }

    public void playSettingButtonSound()
    {

        if (soundAudioPlayer!=null && soundAudioPlayer.isPlaying)
        {
            soundAudioPlayer.Stop();
        }

        if (settingButtonAudio != null)
        {
            soundAudioPlayer.clip = settingButtonAudio;

            if (PlayerPrefs.HasKey(soundVolumeStringKey))
            {
                soundVolume = PlayerPrefs.GetInt(soundVolumeStringKey);
            }

            soundAudioPlayer.volume = soundVolume / 100.0f;

            soundAudioPlayer.Play();
        }
    }

    public void setMusicVolume(int volume)
    {
        Debug.Log("Music player is playing? " + musicAudioPlayer.isPlaying + " volume is " + volume);

        this.musicVolume = volume;

        if(musicAudioPlayer.isPlaying)
        {
            musicAudioPlayer.volume = musicVolume / 100.0f;
        }
    }
}
