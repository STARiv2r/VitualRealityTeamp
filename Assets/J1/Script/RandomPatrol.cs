using UnityEngine;
using UnityEngine.AI;

public class RandomPatrol : MonoBehaviour
{
    public float patrolRadius = 10f;         // NPC가 이동할 랜덤 반경
    public float patrolInterval = 3f;        // 다음 이동까지 대기 시간
    public float hitReactTime = 1f;          // 충돌 후 멈추는 시간

    private NavMeshAgent agent;              // 이동을 위한 NavMesh 에이전트
    private Animator animator;               // 애니메이션 컨트롤러
    private float timer;                     // 시간 체크용 변수
    private float stuckTimer;                // 멈춤 상태 지속 시간 타이머
    private bool isHitReacting = false;      // 충돌 후 멈춰있는 상태 플래그

    // 문 관련 상태 관리용
    private Vector3 originalDestination;
    private Door targetDoor = null;
    private bool isInteractingWithDoor = false;

    private float doorCheckInterval = 10f;  // 문 탐지 주기 (초)
    private float lastDoorCheckTime = -999f; // 마지막 문 탐지 시점

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();      // 에이전트 컴포넌트 가져오기
        animator = GetComponent<Animator>();       // 애니메이터 가져오기
        SetRandomDestination();                    // 처음 목적지 설정
    }

    void Update()
    {
        if (isHitReacting)
        {
            if (animator != null)
                animator.SetFloat("Speed", 0f);
            return;                 // 충돌 반응 중이면 아무것도 하지 않음
        }


        // ✅ 일정 주기로만 문 탐지 시도
        if (!isInteractingWithDoor && targetDoor == null && Time.time - lastDoorCheckTime >= doorCheckInterval)
        {
            lastDoorCheckTime = Time.time;
            CheckAndSetDoorAsDestination();
        }

        // 문 앞 도착 시 문 열기
        if (isInteractingWithDoor && targetDoor != null)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {

                StartCoroutine(InteractWithDoor());
            }
            return;
        }

        timer += Time.deltaTime;


        /*
        // 현재 속도를 애니메이터 Speed 파라미터에 전달 (애니메이션 상태 변경용)
        if (animator != null && agent != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        */

        if (animator != null && agent != null)
        {
            float moveSpeed = agent.velocity.magnitude;

            if (agent.remainingDistance > 0.1f && moveSpeed < 0.1f && agent.hasPath)
            {
                // 경로는 있는데 속도가 거의 0이면 이동 중으로 보정
                moveSpeed = 0.5f;
            }

            animator.SetFloat("Speed", moveSpeed);
        }


        // 일정 시간마다 또는 목적지에 도착하면 새 목적지 설정
        if (!agent.pathPending && agent.remainingDistance < 0.5f || timer >= patrolInterval)
        {
            //TryOpenNearbyDoor(); // 문 열기 시도
            SetRandomDestination();
            timer = 0f;
            stuckTimer = 0f;
            return;
        }

        // 1초 이상 멈춰 있으면 경로 재설정
        if (!agent.pathPending && agent.hasPath && agent.velocity.sqrMagnitude < 1f)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= 3f)
            {
                Debug.Log("3초 이상 멈춤: 경로 재설정");
                SetRandomDestination();
                stuckTimer = 0f;
                timer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

    }

    void SetRandomDestination()
    {
        // 랜덤 방향을 계산해서 목적지 설정
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }


    void OnCollisionEnter(Collision collision)
    {

        // NPC가 아닌 경우 충돌 무시
        if (!collision.gameObject.CompareTag("NPC"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            return;
        }

        // NPC 태그를 가진 오브젝트와 충돌한 경우
        if (collision.gameObject.CompareTag("NPC"))
        {
            Debug.Log("NPC와 충돌함! GetHit 애니메이션 실행 시도");

            StartCoroutine(PlayGetHit());
        }
    }

    private System.Collections.IEnumerator PlayGetHit()
    {
        isHitReacting = true;               // 이동 일시 정지
        agent.isStopped = true;             // NavMesh 에이전트 멈춤
        agent.ResetPath();

        // GetHit 트리거를 발동시켜 애니메이션 전환 시도
        if (animator != null)
        {
            //Debug.Log("SetTrigger: GetHit 호출");
            animator.SetTrigger("GetHit");
        }

        yield return new WaitForSeconds(hitReactTime);

        agent.isStopped = false;            // 다시 이동 재개
        SetRandomDestination(); // 충돌 후 경로 재계산
        isHitReacting = false;
    }

    private void TryOpenNearbyDoor()
    {
        float detectionRadius = 1f; // 문 탐지 반경 (필요에 따라 조정)
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var hit in hits)
        {
            Door door = hit.GetComponent<Door>();
            if (door != null)
            {
                Debug.Log("NPC: 문 발견 → 열기 시도");
                door.MoveMyDoor();  // 문 열기
                return; // 하나만 열고 종료
            }
        }
    }
    
    private bool CheckAndSetDoorAsDestination()
    {
        float detectionRadius = 3f;
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var hit in hits)
        {
            Door door = hit.GetComponent<Door>();
            if (door != null)
            {
                Door.DoorGet doorData = door.UseDoors.Find(d => d.Door == door.gameObject);
                if (doorData == null || doorData.isDoorOpen) continue;

                Debug.Log($"[{name}] 문 감지 → {door.name} 앞으로 이동 시작");

                targetDoor = door;
                originalDestination = agent.destination; // 원래 목적지 기억
                agent.SetDestination(door.transform.position); // 문 앞으로 이동
                isInteractingWithDoor = true;
                return true;
            }
        }
        return false;
    }


    /*
    private System.Collections.IEnumerator InteractWithDoor()
    {
        isInteractingWithDoor = false; // 중복 실행 방지
        agent.isStopped = true;

        Debug.Log($"[{name}] 문 앞 도착 → 문 열기 시도");

        // 문이 닫혀 있다면 열기 반복 시도
        while (!targetDoor.UseDoors[0].isDoorOpen) // 여러 문 있을 경우 조건 조절
        {
            targetDoor.MoveMyDoor();
            yield return new WaitForSeconds(0.2f); // 너무 자주 호출 방지
        }

        //targetDoor.MoveMyDoor();

        // 문이 열리는 데 걸리는 시간만큼 대기 (예: 1.5초)
        yield return new WaitForSeconds(1.5f); // 필요시 Door.cs에서 정확한 시간 계산도 가능

        Debug.Log("문 열림 완료 → 이동 재개");
        agent.isStopped = false;
        agent.SetDestination(originalDestination);
        targetDoor = null;
    }
    */

    private System.Collections.IEnumerator InteractWithDoor()
    {
        isInteractingWithDoor = false;
        agent.isStopped = true;

        if (targetDoor == null || targetDoor.UseDoors.Count == 0)
            yield break;

        var doorData = targetDoor.UseDoors.Find(d => d.Door == targetDoor.gameObject);
        if (doorData == null)
            yield break;

        Debug.Log($"[{name}] 문 앞 도착 → 문 열기 시도");
        targetDoor.MoveMyDoor(); // 단 한 번만 호출 (반복 금지)

        // 문 열림까지 기다리기 (충분한 시간 확보)
        yield return new WaitForSeconds(1.5f);

        // ✅ NavMeshLink를 통과해 이동할 "문 반대편 지점"을 설정해야 함
        Vector3 exitOffset = targetDoor.transform.forward * 0.1f; //
        Vector3 exitPoint = targetDoor.transform.position + exitOffset;

        // Debug 용 시각화
        Debug.DrawRay(targetDoor.transform.position, exitOffset, Color.cyan, 3f);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(exitPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            Debug.Log($"[{name}] 문 열림 완료 → 반대편({hit.position})으로 이동 재개");
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning($"[{name}] 반대편 NavMesh 위치 찾기 실패 → 원래 목적지로 이동");
            agent.isStopped = false;
            agent.SetDestination(originalDestination);
        }

        targetDoor = null;

        // 일정 시간 후 문 닫기 코루틴 실행
        StartCoroutine(CloseDoorAfterDelay(targetDoor, 3f)); // 3초 후 닫기

    }

    private System.Collections.IEnumerator CloseDoorAfterDelay(Door door, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (door == null || door.UseDoors.Count == 0)
            yield break;

        var doorData = door.UseDoors.Find(d => d.Door == door.gameObject);
        if (doorData == null || !doorData.isDoorOpen)
            yield break;

        Debug.Log($"[{name}] 문 자동 닫기 시도");

        doorData.isDoorOpen = false;
        door.door_in_use = true;
        StartCoroutine(door.OpenDoor(doorData.CloseValue, doorData.Door, doorData.RotationOrigin));
    }


}
