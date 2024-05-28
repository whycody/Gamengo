using UnityEngine;

public class AudioRegister : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioManager.Instance.RegisterAudioSource(audioSource);
    }
}
