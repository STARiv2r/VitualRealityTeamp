using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class HapticFeedback : MonoBehaviour
{
    public HapticImpulsePlayer leftHapticPlayer;
    public HapticImpulsePlayer rightHapticPlayer;

    public void TriggerHaptic(float intensity)
    {
        float amplitude = Mathf.Clamp01(intensity);
        float duration = 0.1f; // 100ms

        if (leftHapticPlayer != null)
            leftHapticPlayer.SendHapticImpulse(amplitude, duration);

        if (rightHapticPlayer != null)
            rightHapticPlayer.SendHapticImpulse(amplitude, duration);
    }

    public void StopHaptic()
    {
        if (leftHapticPlayer != null)
            leftHapticPlayer.SendHapticImpulse(0f, 0.1f);

        if (rightHapticPlayer != null)
            rightHapticPlayer.SendHapticImpulse(0f, 0.1f);
    }
}
