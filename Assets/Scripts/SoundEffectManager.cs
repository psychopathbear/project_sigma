using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if(audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    // Stop current audio
    public static void Stop()
    {
        audioSource.Stop();
    }

    void Start()
    {
        float savedEffectVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        SetVolume(savedEffectVolume);

        if(sfxSlider != null)
        {
            sfxSlider.value = savedEffectVolume;
            sfxSlider.onValueChanged.AddListener(delegate { SetVolume(sfxSlider.value); });
        }
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }

    public static implicit operator SoundEffectManager(BossSoundEffectManager v)
    {
        throw new NotImplementedException();
    }
}
