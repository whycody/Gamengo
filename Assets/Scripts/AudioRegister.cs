using UnityEngine;

public class AudioRegister : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        AudioManager.Instance.RegisterAudioSource(_audioSource);
    }
}
