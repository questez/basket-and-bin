using UnityEngine;
using UnityEngine.UI;

public class AudioManager: MonoBehaviour
{
    [SerializeField] private Slider SoundSlider;

    private AudioSource[] AllAudioSources;

    public static float TotalSoundVolume { get; private set; } = 0.75f;


    private void Start()
    {
        if (SoundSlider != null)
        {
            SoundSlider.value = TotalSoundVolume;
        }
        AllAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        SetVolume(AllAudioSources);
    }

    private void OnEnable()
    {
        if (SoundSlider != null)
        {
            SoundSlider.onValueChanged.AddListener(ChangeVolume);
        }
    }

    private void OnDisable()
    {
        if (SoundSlider != null)
        {
            SoundSlider.onValueChanged.RemoveListener(ChangeVolume);
        }
    }
    

    private void ChangeVolume(float value)
    {
        TotalSoundVolume = value;
        Debug.Log($"TotalSoundVolume: {TotalSoundVolume}");
        SetVolume(AllAudioSources);
    }

    private void SetVolume(AudioSource[] sources)
    {
        if (sources != null)
        {
            foreach (AudioSource source in sources)
            {
                source.volume = TotalSoundVolume;
            }
        }        
    }
}
