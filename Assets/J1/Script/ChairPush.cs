using System.Collections;
using UnityEngine;

public class ChairPush : MonoBehaviour
{
    public float backDistance = 0.5f;     // 뒤로 밀릴 거리
    public float moveDuration = 1f;       // 밀리는 데 걸리는 시간
    public float delay = 0.2f;            // 캐릭터가 일어난 후 딜레이

    public void PushBack(Vector3 facingDirection)
    {
        Debug.Log("ChairPush: PushBack 호출됨");
        Vector3 pushDir = -facingDirection.normalized;
        Vector3 targetPosition = transform.position + pushDir * backDistance;
        StartCoroutine(MoveToPositionSmoothly(targetPosition));
    }

    private IEnumerator MoveToPositionSmoothly(Vector3 targetPos)
    {
        yield return new WaitForSeconds(delay);

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }
}
