using UnityEngine;

public class HitSound : MonoBehaviour
{
    public AudioClip hitClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            if (hitClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitClip, 0.4f);
                Debug.Log("Ãæµ¹À½!");
            }
        }
    }
}
