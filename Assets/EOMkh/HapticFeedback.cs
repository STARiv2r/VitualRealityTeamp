using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class HapticFeedback : MonoBehaviour
{
    
    //public HapticImpulsePlayer leftHapticPlayer;
    //public HapticImpulsePlayer rightHapticPlayer;

    

    public void TriggerHaptic(float intensity)
    {
        float amplitude = Mathf.Clamp01(intensity);
        float duration = 0.1f; // 100ms

        OVRInput.SetControllerVibration(duration, amplitude, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(duration, amplitude, OVRInput.Controller.RTouch);
        
 
    }

    public void StopHaptic()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
