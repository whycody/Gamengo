using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private List<AudioSource> audioSources = new List<AudioSource>();
    public static float volume = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        UpdateVolume();
    }

    public void RegisterAudioSource(AudioSource source)
    {
        audioSources.Add(source);
        source.volume = volume;
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        foreach (var source in audioSources)
        {
            source.volume = volume;
        }
    }

    public float GetVolume()
    {
        return volume;
    }
}
