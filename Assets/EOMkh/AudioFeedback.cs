using UnityEngine;

public class AudioFeedback : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip fireAlarm;
    public AudioClip toolGrab;
    public AudioClip doorPush;

    public void PlaySound(string key, float volume)
    {
        switch (key)
        {
            case "FireAlarm": audioSource.PlayOneShot(fireAlarm, volume); break;
            case "ToolGrab": audioSource.PlayOneShot(toolGrab, volume); break;
            case "DoorPush": audioSource.PlayOneShot(doorPush, volume); break;
        }
    }
}
