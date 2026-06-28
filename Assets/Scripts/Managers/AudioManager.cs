using UnityEngine;

/// Handles all music and sound effects for Vector Wars.
/// 
/// This version supports:
/// - Saved music volume
/// - Saved SFX volume
/// 
/// Music can effectively be turned off by setting music volume to 0.
public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clips")]

    [SerializeField] private AudioClip gameplayMusic;

    [Header("Sound Effects")]

    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip spreadShotSFX;
    [SerializeField] private AudioClip enemyExplosionSFX;
    [SerializeField] private AudioClip playerHitSFX;
    [SerializeField] private AudioClip xpPickupSFX;
    [SerializeField] private AudioClip levelUpSFX;
    [SerializeField] private AudioClip victorySFX;
    [SerializeField] private AudioClip gameOverSFX;
    [SerializeField] private AudioClip buttonClickSFX;


    [Header("Default Audio Settings")]

    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.25f;

    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 0.6f;

    public static AudioManager Instance { get; private set; }

    private const string MusicVolumeKey = "VectorWars_MusicVolume";
    private const string SFXVolumeKey = "VectorWars_SFXVolume";

    private void Awake()
    {
        // Singleton setup.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadAudioSettings();
        SetupMusicSource();
        ApplyAudioSettings();
    }

    private void Start()
    {
        PlayGameplayMusic();
    }

    /// Prepares the music AudioSource.
    private void SetupMusicSource()
    {
        if (musicSource == null)
        {
            Debug.LogWarning("AudioManager is missing Music Source.");
            return;
        }

        if (gameplayMusic != null)
        {
            musicSource.clip = gameplayMusic;
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;
    }

    /// Starts gameplay music.
    public void PlayGameplayMusic()
    {
        if (musicSource == null)
        {
            return;
        }

        if (musicSource.clip == null && gameplayMusic != null)
        {
            musicSource.clip = gameplayMusic;
        }

        musicSource.loop = true;
        musicSource.volume = musicVolume;

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    /// Changes music volume from the UI slider.
    public void SetMusicVolume(float newVolume)
    {
        musicVolume = Mathf.Clamp01(newVolume);

        ApplyAudioSettings();
        SaveAudioSettings();
    }

    /// Changes SFX volume from the UI slider.
    public void SetSFXVolume(float newVolume)
    {
        sfxVolume = Mathf.Clamp01(newVolume);

        ApplyAudioSettings();
        SaveAudioSettings();
    }

    /// Applies the current audio settings to the AudioSources.
    public void ApplyAudioSettings()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    /// Saves audio settings.
    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        PlayerPrefs.SetFloat(SFXVolumeKey, sfxVolume);
        PlayerPrefs.Save();

        Debug.Log("Audio settings saved. Music: " + musicVolume + " | SFX: " + sfxVolume);
    }

    /// Loads saved audio settings.
    private void LoadAudioSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, musicVolume);
        sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, sfxVolume);

        Debug.Log("Audio settings loaded. Music: " + musicVolume + " | SFX: " + sfxVolume);
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayShootSFX()
    {
        PlaySFX(shootSFX);
    }

    public void PlayEnemyExplosionSFX()
    {
        PlaySFX(enemyExplosionSFX);
    }

    public void PlayPlayerHitSFX()
    {
        PlaySFX(playerHitSFX);
    }

    public void PlayXPPickupSFX()
    {
        PlaySFX(xpPickupSFX);
    }

    public void PlayLevelUpSFX()
    {
        PlaySFX(levelUpSFX);
    }

    public void PlayVictorySFX()
    {
        PlaySFX(victorySFX);
    }

    public void PlayGameOverSFX()
    {
        PlaySFX(gameOverSFX);
    }

    public void PlayButtonClickSFX()
    {
        PlaySFX(buttonClickSFX);
    }

    public void PlaySpreadShotSFX()
    {
        PlaySFX(spreadShotSFX);
    }
}