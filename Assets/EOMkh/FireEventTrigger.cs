using UnityEngine;

public class FireEventTrigger : MonoBehaviour
{
    public FeedbackManager feedbackManager;
    public Transform playerTransform;

    public float maxDistance = 10f; // 최대 거리 (이 거리 밖에서는 효과 없음)

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // 거리 기반 intensity 계산
        float intensity = Mathf.Clamp01(1f - (distance / maxDistance));

        // 각각 독립적으로 적용 (예시: 시각/청각/햅틱 동일 적용 가능)
        feedbackManager.TriggerVisualFeedback(intensity);
        feedbackManager.TriggerAudioFeedback(intensity);
        feedbackManager.TriggerHapticFeedback(intensity);
    }

    public void OnPlayerEscape()
    {
        feedbackManager.StopVisualFeedback();
        feedbackManager.StopAudioFeedback();
        feedbackManager.StopHapticFeedback();
    }
}
