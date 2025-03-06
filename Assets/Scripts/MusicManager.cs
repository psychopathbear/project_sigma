using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance; 
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if(backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }

        // Load volume settings from PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SetVolume(savedVolume);

        // If you have a slider in the game scene as well, update it here
        if (musicSlider != null)
        {
            musicSlider.value = savedVolume;
            musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
        }
    }

    public static void SetVolume(float volume)
    {
        if (instance != null)
        {
            instance.audioSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume); // Save the change immediately
        }
    }

    public static void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if(instance != null)
        {
            if(audioClip != null)
            {
                instance.audioSource.clip = audioClip;   
            }
            if(instance.audioSource.clip != null)
            {
                if(resetSong)
                {
                    instance.audioSource.Stop();
                }
                instance.audioSource.Play();
            }
        }
    }

    public static void ResumeBackgroundMusic()
{
    if (instance != null)
    {
        instance.audioSource.UnPause();
    }
}

    public static void PauseBackgroundMusic()
    {
        if(instance != null)
        {
            instance.audioSource.Pause();
        }
    }
}