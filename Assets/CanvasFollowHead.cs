using UnityEngine;

public class CanvasFollowHead : MonoBehaviour
{
    public Transform playerHead;
    public float distance = 0.5f;
    public Vector3 offset = Vector3.zero;

    void LateUpdate()
    {
        Vector3 targetPosition = playerHead.position + playerHead.forward * distance + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

        Quaternion targetRotation = Quaternion.LookRotation(playerHead.forward, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}
