using System.Collections;
using Unity.Collections;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public VisualFeedback visualFeedback;
    public AudioFeedback audioFeedback;
    public HapticFeedback hapticFeedback;

    // 단일 피드백 켜기
    public void TriggerVisualFeedback(float intensity)
    {
        if (visualFeedback != null)
            visualFeedback.ApplyVisualEffect(intensity);
    }

    public void TriggerAudioFeedback(float intensity)
    {
        if (audioFeedback != null)
            audioFeedback.PlayAudio(intensity);
    }

    public void TriggerHapticFeedback(float intensity)
    {
        if (hapticFeedback != null)
            hapticFeedback.TriggerHaptic(intensity);
    }
    public void TriggerHapticFeedback(float intensity, float time)
    {
        if (hapticFeedback != null)
            hapticFeedback.TriggerHaptic(intensity);

        StartCoroutine(StopByTime(time));
    }

    // 단일 피드백 끄기
    public void StopVisualFeedback()
    {
        if (visualFeedback != null)
            visualFeedback.StopVisualEffect();
    }

    public void StopAudioFeedback()
    {
        if (audioFeedback != null)
            audioFeedback.StopAudio();
    }

    public void StopHapticFeedback()
    {
        if (hapticFeedback != null)
            hapticFeedback.StopHaptic();
    }

    IEnumerator StopByTime(float t)
    {
        yield return new WaitForSeconds(t);

        StopHapticFeedback();
    }
}
