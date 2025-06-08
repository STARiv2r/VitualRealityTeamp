using UnityEngine;
using UnityEngine.UI;

public class VisualFeedback : MonoBehaviour
{
    // 예시로 화면에 붉은 투명 이미지 오버레이 사용 (Canvas Group 등 활용 가능)
    [SerializeField] private Image redOverlay;
    private Color currentColor;

    private void Start()
    {
        currentColor = redOverlay.color;
    }
    public void ApplyVisualEffect(float intensity)
    {
        
        
        
        if (redOverlay != null)
        {
            currentColor.a = Mathf.Clamp01(intensity*0.7f);
            redOverlay.color = currentColor;
        }
    }

    public void StopVisualEffect()
    {
        if (redOverlay != null)
        {
            currentColor.a = 0f;
            redOverlay.color = currentColor;
        }
    }
}
