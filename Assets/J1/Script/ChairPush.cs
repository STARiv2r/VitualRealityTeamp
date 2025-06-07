using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChairPush : MonoBehaviour
{
    public float pushForce = 200f; // �и��� ��
    public float pushDuration = 0.1f; // �и��� �ð�
    private Rigidbody rb;
    private bool isPushed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Push(Vector3 direction)
    {
        if (isPushed) return; // �ߺ� ����
        isPushed = true;

        // �и� ����
        rb.isKinematic = false;
        rb.AddForce(direction.normalized * pushForce);

        // ���� �ð� �� �ٽ� ���� ó��
        Invoke(nameof(StopMovement), pushDuration);
    }

    private void StopMovement()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; // �ٽ� ����
    }
}
