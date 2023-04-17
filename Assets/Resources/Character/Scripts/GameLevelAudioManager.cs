using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelAudioManager : MonoBehaviour {


    public enum audio
    {
        punched,
        shotByAntibody,
        shotByAcid,
        slashed,
        munching,
        shoot,
        dash,
        swoosh,
        activation,
        spawn,
        reload,
        victory,
        background,
        ulti,
        notification,
        healing
    };

    [SerializeField]
    private AudioClip beatenSound;

    [SerializeField]
    private AudioClip gettingShotByAntibodySound;

    [SerializeField]
    private AudioClip gettingShotByAcidSound;

    [SerializeField]
    private AudioClip gettingSlashedSound;

    [SerializeField]
    private AudioClip munchingSound;

    [SerializeField]
    private AudioClip gunShotSound;

    [SerializeField]
    private AudioClip victorySound;

    [SerializeField]
    private AudioClip backgroundMusic;

    [SerializeField]
    private AudioClip ultiMusic;

    [SerializeField]
    private AudioClip swordSwooshSound;

    [SerializeField]
    private AudioClip dashSound;

    [SerializeField]
    private AudioClip activationSound;

    [SerializeField]
    private AudioClip spawnSound;

    [SerializeField]
    private AudioClip gunReloadSound;

    [SerializeField]
    private AudioClip healingSound;

    [SerializeField]
    private AudioClip notiSound;







    [SerializeField]
    private AudioSource beatenSoundSource;

    [SerializeField]
    private AudioSource gettingShotByAntibodySoundSource;

    [SerializeField]
    private AudioSource gettingShotByAcidSoundSource;

    [SerializeField]
    private AudioSource gettingSlashedSoundSource;

    [SerializeField]
    private AudioSource munchingSoundSource;

    [SerializeField]
    private AudioSource gunShotSoundSource;

    [SerializeField]
    private AudioSource victorySoundSource;

    [SerializeField]
    private AudioSource backgroundMusicSource;

    [SerializeField]
    private AudioSource ultiMusicSource;

    [SerializeField]
    private AudioSource neutrophilAttackSoundSource;

    [SerializeField]
    private AudioSource turretSoundSource;

    [SerializeField]
    private AudioSource allySoundSource;

    [SerializeField]
    private AudioSource helperSoundSource;

    [SerializeField]
    private AudioSource notiSoundSource;


    static public AudioSource soundSourceBeaten;

    static public AudioSource soundSourceGettingShotByAntibody;

    static public AudioSource soundSourceGettingShotByAcid;

    static public AudioSource soundSourceGettingSlashed;

    static public AudioSource soundSourceMunching;

    static public AudioSource soundSourceGunShot;

    static public AudioSource soundSourceVictory;

    static public AudioSource musicSourceBackground;

    static public AudioSource musicSourceUlti;

    static public AudioSource soundSourceNeutrophilAttack;

    static public AudioSource soundSourceTurret;

    static public AudioSource soundSourceAlly;

    static private AudioSource soundSourceHelper;

    static private AudioSource soundSourceNoti;




    static private AudioSource playingAudioSource;

    static private int soundVolume;

    static private int musicVolume;

    private string soundVolumeStringKey = "Sound Volume";

    private string musicVolumeStringKey = "Music Volume";

    static private float currentVolume;



    static private AudioClip soundSwordSwoosh;

    static private AudioClip soundDash;

    static private AudioClip soundSpawn;

    static private AudioClip soundGunReload;

    static private AudioClip soundHealing;

    void Awake()
    {



        soundSwordSwoosh = swordSwooshSound;

        soundDash = dashSound;

        soundSpawn = spawnSound;

        soundGunReload = gunReloadSound;

        soundHealing = healingSound;

        if (!PlayerPrefs.HasKey(soundVolumeStringKey))
        {
            PlayerPrefs.SetInt(soundVolumeStringKey, 75);
        }

        soundVolume = PlayerPrefs.GetInt(soundVolumeStringKey);

        if (!PlayerPrefs.HasKey(musicVolumeStringKey))
        {
            PlayerPrefs.SetInt(musicVolumeStringKey, 75);
        }

        musicVolume = PlayerPrefs.GetInt(musicVolumeStringKey);

        soundSourceBeaten = beatenSoundSource;
        soundSourceGettingShotByAntibody = gettingShotByAntibodySoundSource;
        soundSourceGettingShotByAcid=gettingShotByAcidSoundSource;
        soundSourceGettingSlashed=gettingSlashedSoundSource;
        soundSourceMunching=munchingSoundSource;
        soundSourceGunShot=gunShotSoundSource;
        soundSourceVictory=victorySoundSource;
        soundSourceHelper = helperSoundSource;
        musicSourceBackground = backgroundMusicSource;
        musicSourceUlti = ultiMusicSource;
        soundSourceNoti = notiSoundSource;

        soundSourceNeutrophilAttack = neutrophilAttackSoundSource;

        soundSourceTurret = turretSoundSource;

        soundSourceAlly = allySoundSource;

        soundSourceBeaten.clip = beatenSound;
        soundSourceGettingShotByAntibody.clip = gettingShotByAntibodySound;
        soundSourceGettingShotByAcid.clip = gettingShotByAcidSound;
        soundSourceGettingSlashed.clip = gettingSlashedSound;
        soundSourceMunching.clip = munchingSound;
        soundSourceGunShot.clip = gunShotSound;
        soundSourceVictory.clip = victorySound;
        soundSourceNeutrophilAttack.clip = swordSwooshSound;
        soundSourceTurret.clip = activationSound;
        soundSourceAlly.clip = spawnSound;
        soundSourceHelper.clip = healingSound;
        musicSourceBackground.clip = backgroundMusic;
        soundSourceNoti.clip = notiSound;
        musicSourceUlti.clip = ultiMusic;

        if(backgroundMusic!=null && musicSourceBackground!=null)
        {
            musicSourceBackground.volume = musicVolume / 100.0f;
            musicSourceBackground.Play();
        }

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static public void playSound(audio sound, float distanceFromPlayer)
    {



        switch (sound)
        {
            case audio.punched:
                {
                    playingAudioSource = soundSourceBeaten;
                    //Debug.Log(distanceFromPlayer);
                    break;
                }
            case audio.shoot:
                {
                    playingAudioSource = soundSourceGunShot;
                    break;
                }
            case audio.shotByAcid:
                {
                    playingAudioSource = soundSourceGettingShotByAcid;
                    break;
                }
            case audio.shotByAntibody:
                {
                    playingAudioSource = soundSourceGettingShotByAntibody;
                    break;
                }
            case audio.slashed:
                {
                    playingAudioSource = soundSourceGettingSlashed;
                    break;
                }
            case audio.victory:
                {
                    if(musicSourceBackground.isPlaying)
                    {
                        musicSourceBackground.Stop();
                    }

                    if (musicSourceUlti.isPlaying)
                    {
                        musicSourceUlti.Stop();
                    }

                    playingAudioSource = soundSourceVictory;
                    break;
                }
            case audio.munching:
                {
                    playingAudioSource = soundSourceMunching;
                    break;
                }
            case audio.dash:
                {
                    playingAudioSource = soundSourceNeutrophilAttack;
                    playingAudioSource.clip = soundDash;
                    break;
                }
            case audio.swoosh:
                {
                    playingAudioSource = soundSourceNeutrophilAttack;
                    playingAudioSource.clip = soundSwordSwoosh;
                    break;
                }
            case audio.activation:
                {
                    playingAudioSource = soundSourceTurret;
                    break;
                }
            case audio.spawn:
                {
                    playingAudioSource = soundSourceAlly;
                    playingAudioSource.clip = soundSpawn;
                    break;
                }
            case audio.reload:
                {
                    playingAudioSource = soundSourceAlly;
                    playingAudioSource.clip = soundGunReload;
                    break;
                }
            case audio.healing:
                {
                    playingAudioSource = soundSourceHelper;
                    playingAudioSource.clip = soundHealing;
                    
                    break;
                }
            case audio.notification:
                {
                    playingAudioSource = soundSourceNoti;

                    break;
                }

        }

        if (playingAudioSource != null && playingAudioSource.clip != null)
        {

            currentVolume = ((-1.0f / 30.0f) * distanceFromPlayer + 1.0f);

            if (currentVolume<0)
            {
                currentVolume = 0;
            }

            playingAudioSource.volume = currentVolume * (soundVolume / 100.0f);

            //Debug.Log("Distance from player: " + distanceFromPlayer + " volume = " + (currentVolume * (soundVolume / 100.0f)));



            if (playingAudioSource.isPlaying)
            {
                playingAudioSource.Stop();
            }

            playingAudioSource.Play();
        }
    }

    static public void pauseBGMandPlay(audio sound)
    {
        switch (sound)
        {
            case audio.ulti:
                {

                    if (musicSourceUlti != null && musicSourceUlti.clip!=null)
                    {
                        playingAudioSource = musicSourceUlti;

                        if (musicSourceBackground.isPlaying)
                        {
                            musicSourceBackground.Pause();
                        }

                        if(playingAudioSource.isPlaying)
                        {
                            playingAudioSource.Stop();
                        }

                        playingAudioSource.Play();
                    }

                    break;
                }
        }
    }

    static public void resumeBGMandStop(audio sound)
    {



        switch (sound)
        {
            case audio.ulti:
                {

                    if (musicSourceUlti != null && musicSourceUlti.clip != null)
                    {
                        playingAudioSource = musicSourceUlti;

                        musicSourceBackground.UnPause();

                        if (playingAudioSource.isPlaying)
                        {
                            playingAudioSource.Stop();
                        }

                    }

                    break;
                }
        }
    }

    static public void adjustMusicVolume(int volume)
    {
        musicVolume = volume;

        if(musicSourceBackground.isPlaying)
        {
            musicSourceBackground.volume = musicVolume / 100.0f;
        }
    }
}
