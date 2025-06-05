using UnityEngine;

public class AudioFeedback : MonoBehaviour
{
    [SerializeField] private AudioSource fireAudioSource;

    public void PlayAudio(float intensity)
    {
        if (fireAudioSource != null)
        {
            if (!fireAudioSource.isPlaying)
                fireAudioSource.Play();

            fireAudioSource.volume = Mathf.Clamp01(intensity);
        }
    }

    public void StopAudio()
    {
        if (fireAudioSource != null && fireAudioSource.isPlaying)
        {
            fireAudioSource.Stop();
        }
    }
}
