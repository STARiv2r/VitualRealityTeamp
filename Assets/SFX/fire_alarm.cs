using UnityEngine;

public class fire_alarm : MonoBehaviour
{
	public AudioSource audioSource;
	public float delaySeconds = 5f;
	
	void Start() {
		Invoke("PlaySound", delaySeconds);
	}

	void PlaySound() {
		if (audioSource != null) {
			audioSource.Play();
		}
	}
}
