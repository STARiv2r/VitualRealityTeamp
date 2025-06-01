using UnityEngine;
using UnityEngine.XR;

public class HapticFeedback : MonoBehaviour
{
    public XRNode handNode;

    public void Trigger(float amplitude, float duration)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(handNode);
        if (device.TryGetHapticCapabilities(out var cap) && cap.supportsImpulse)
        {
            device.SendHapticImpulse(0, amplitude, duration);
        }
    }
}
