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

        // ✅ 문 탐지 우선 실행
        if (!isInteractingWithDoor && !agent.pathPending && agent.remainingDistance >= 0.5f)
        {
            if (CheckAndSetDoorAsDestination()) return;
        }

        // 문 앞 도착 시 문 열기
        if (isInteractingWithDoor && targetDoor != null)
        {
            if (!agent.pathPending && agent.remainingDistance < 1.0f)
            {
                /*
                Debug.Log("문 앞 도착 → 문 열기 시도");
                targetDoor.MoveMyDoor();
                targetDoor = null;
                isInteractingWithDoor = false;
                // 다시 원래 목적지로 이동
                agent.SetDestination(originalDestination);
                */
                StartCoroutine(InteractWithDoor());
            }
            return;
        }

        timer += Time.deltaTime;

        // 현재 속도를 애니메이터 Speed 파라미터에 전달 (애니메이션 상태 변경용)
        if (animator != null && agent != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
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
        /*
        if (collision.gameObject.name.Contains("Office building")) return;
        // 충돌했을 때 어떤 오브젝트와 충돌했는지 출력
        Debug.Log($"충돌 대상: {collision.gameObject.name}");
        */

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
        float detectionRadius = 5f;
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var hit in hits)
        {
            Door door = hit.GetComponent<Door>();
            if (door != null)
            {
                Debug.Log("문 감지 → 문 앞으로 이동 시작");
                targetDoor = door;
                originalDestination = agent.destination; // 원래 목적지 기억
                agent.SetDestination(door.transform.position); // 문 앞으로 이동
                isInteractingWithDoor = true;
                return true;
            }
        }
        return false;
    }

    private System.Collections.IEnumerator InteractWithDoor()
    {
        isInteractingWithDoor = false; // 중복 실행 방지
        agent.isStopped = true;

        Debug.Log("문 앞 도착 → 이동 멈추고 문 열기 시도");

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

}
