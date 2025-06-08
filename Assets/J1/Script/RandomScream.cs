using UnityEngine;

public class RandomScream : MonoBehaviour
{
    public AudioClip screamClip;
    public float minInterval = 5f;     // 최소 간격 (초)
    public float maxInterval = 15f;    // 최대 간격 (초)

    private AudioSource audioSource;
    private float nextScreamTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ScheduleNextScream(); // 첫 비명 예약
    }

    void Update()
    {
        if (Time.time >= nextScreamTime)
        {
            TryScream();
            ScheduleNextScream(); // 다음 비명 예약
        }
    }

    void TryScream()
    {
        if (audioSource != null && screamClip != null /*&& !audioSource.isPlaying*/)
        {
            audioSource.PlayOneShot(screamClip);
            Debug.Log("비명!");
        }
    }

    void ScheduleNextScream()
    {
        float delay = Random.Range(minInterval, maxInterval);
        nextScreamTime = Time.time + delay;
    }
}
