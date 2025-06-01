using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public HapticFeedback haptic;
    public VisualFeedback visual;
    public AudioFeedback audio;

    public void TriggerFeedback(FeedbackType type, float intensity)
    {
        switch (type)
        {
            case FeedbackType.Fire:
                visual.UpdateColorOverlay(intensity);
                haptic.Trigger(intensity, 0.1f);
                audio.PlaySound("FireAlarm", intensity);
                break;

            case FeedbackType.GrabTool:
                haptic.Trigger(intensity, 0.05f);
                audio.PlaySound("ToolGrab", 1f);
                break;

            case FeedbackType.PushDoor:
                haptic.Trigger(intensity, 0.08f);
                audio.PlaySound("DoorPush", 0.8f);
                break;
        }
    }
}
public enum FeedbackType
{
    Fire,
    GrabTool,
    PushDoor
}