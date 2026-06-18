using UnityEngine;

/// Handles global audio playback for Vector Wars.
/// 
/// Other scripts can call this manager to play sound effects
/// such as shooting, explosions, XP pickups, level-up, victory, and game over.
public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]

    [Tooltip("AudioSource used for background music.")]
    [SerializeField] private AudioSource musicSource;

    [Tooltip("AudioSource used for sound effects.")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Music Clips")]

    [Tooltip("Main gameplay background music.")]
    [SerializeField] private AudioClip gameplayMusic;

    [Header("Sound Effects")]

    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip enemyExplosionSFX;
    [SerializeField] private AudioClip playerHitSFX;
    [SerializeField] private AudioClip xpPickupSFX;
    [SerializeField] private AudioClip levelUpSFX;
    [SerializeField] private AudioClip victorySFX;
    [SerializeField] private AudioClip gameOverSFX;
    [SerializeField] private AudioClip buttonClickSFX;

    // Simple global access so other scripts can call AudioManager.Instance.
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // If another AudioManager already exists, destroy this duplicate.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PlayGameplayMusic();
    }

    /// Starts looping gameplay music.
    public void PlayGameplayMusic()
    {
        if (musicSource == null || gameplayMusic == null)
        {
            return;
        }

        musicSource.clip = gameplayMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    /// Plays a one-shot sound effect.
    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip);
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
}