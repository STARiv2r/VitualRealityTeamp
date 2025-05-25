using UnityEngine;
using UnityEngine.UI;

public class VisualFeedback : MonoBehaviour
{
    public Image redOverlay;
    

    public void UpdateColorOverlay(float alpha)
    {
        var color = redOverlay.color;
        color.a = Mathf.Lerp(color.a, alpha * 0.6f, Time.deltaTime * 5f);
        redOverlay.color = color;
    }
}
