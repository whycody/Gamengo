using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private List<AudioSource> _audioSources = new List<AudioSource>();
    public static float Volume = 0.5f;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }


    private void Start()
    {
        UpdateVolume();
    }

    public void RegisterAudioSource(AudioSource source)
    {
        _audioSources.Add(source);
        source.volume = Volume;
    }

    public void SetVolume(float newVolume)
    {
        Volume = newVolume;
        PlayerPrefs.SetFloat("volume", Volume);
        PlayerPrefs.Save();
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        foreach (var source in _audioSources)
            source.volume = Volume;
    }

    public float GetVolume()
    {
        return Volume;
    }
}
