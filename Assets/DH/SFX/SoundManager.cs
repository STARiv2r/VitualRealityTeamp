using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip soundToPlay; // 인스펙터에서 할당
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(PlaySoundAfterDelay(5f)); // ⏱️ 5초 후 재생
    }

    IEnumerator PlaySoundAfterDelay(float delay)
    {
        Debug.Log("5초 대기 시작");
        yield return new WaitForSeconds(delay);
        Debug.Log("5초 후 사운드 재생 시도");

        if (soundToPlay != null)
        {
            audioSource.PlayOneShot(soundToPlay);
            Debug.Log("사운드 재생됨: " + soundToPlay.name);
        }
        else
        {
            Debug.LogWarning("사운드가 null입니다");
        }
    }
}