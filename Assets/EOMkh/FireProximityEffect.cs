using UnityEngine;
using UnityEngine.UI;

public class FireProximityEffect : MonoBehaviour
{
    public Transform character;
    public Transform fireSource;
    public Image redOverlay;
    public float maxDistance = 10f;

    void Update()
    {
        float distance = Vector3.Distance(character.position, fireSource.position);
        float intensity = Mathf.Clamp01(1 - (distance / maxDistance));

        Color currentColor = redOverlay.color;
        currentColor.a = intensity * 0.6f; // 최대 알파 60%까지
        redOverlay.color = currentColor;
    }
}
