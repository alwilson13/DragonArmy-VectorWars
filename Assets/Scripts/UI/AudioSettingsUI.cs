using UnityEngine;
using UnityEngine.UI;

/// Connects the audio settings UI to the AudioManager.
/// 
/// This version only uses:
/// - Music volume slider
/// - SFX volume slider
/// 
/// The music toggle was removed because setting music volume to 0
/// already works as a music off option.
public class AudioSettingsUI : MonoBehaviour
{
    [Header("UI References")]

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void OnEnable()
    {
        SyncUIWithAudioManager();
    }

    private void Start()
    {
        SyncUIWithAudioManager();
    }

    /// Updates the sliders to match the saved AudioManager values.
    public void SyncUIWithAudioManager()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioSettingsUI could not find AudioManager.");
            return;
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.GetSFXVolume());
        }
    }

    /// Called by the Music Volume Slider.
    public void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }

    /// Called by the SFX Volume Slider.
    public void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }
}