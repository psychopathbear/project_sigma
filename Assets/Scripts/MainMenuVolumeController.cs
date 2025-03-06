using UnityEngine;
using UnityEngine.UI;

public class MainMenuVolumeController : MonoBehaviour
{
    public Slider musicSlider;  
    public Slider sfxSlider;
    
    private AudioSource audioSource; // Ana menü müziği için AudioSource

    void Awake()
    {
        // AudioSource component'ini al
        audioSource = GetComponent<AudioSource>();
        
        // PlayerPrefs'ten kayıtlı ses değerlerini yükle
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        // Başlangıçta ses seviyesini ayarla
        if (audioSource != null)
        {
            audioSource.volume = musicSlider.value;
        }
    }

    // Müzik ses seviyesini güncelle
    public void OnMusicVolumeChanged()
    {
        if (audioSource != null)
        {
            audioSource.volume = musicSlider.value;
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        }
    }

    // SFX ses seviyesini kaydet
    public void OnSFXVolumeChanged()
    {
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }
}
