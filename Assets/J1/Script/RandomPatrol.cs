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
    private bool isHitReacting = false;      // 충돌 후 멈춰있는 상태 플래그

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();      // 에이전트 컴포넌트 가져오기
        animator = GetComponent<Animator>();       // 애니메이터 가져오기
        SetRandomDestination();                    // 처음 목적지 설정
    }

    void Update()
    {
        if (isHitReacting) return;                 // 충돌 반응 중이면 아무것도 하지 않음

        timer += Time.deltaTime;

        // 현재 속도를 애니메이터 Speed 파라미터에 전달 (애니메이션 상태 변경용)
        if (animator != null && agent != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        // 일정 시간마다 또는 목적지에 도착하면 새 목적지 설정
        if (!agent.pathPending && agent.remainingDistance < 0.5f || timer >= patrolInterval)
        {
            SetRandomDestination();
            timer = 0f;
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
        // 충돌했을 때 어떤 오브젝트와 충돌했는지 출력
        //Debug.Log($"충돌 대상: {collision.gameObject.name}");

        // NPC 태그를 가진 오브젝트와 충돌한 경우
        if (collision.gameObject.CompareTag("NPC"))
        {
            //Debug.Log("NPC와 충돌함! GetHit 애니메이션 실행 시도");
            StartCoroutine(PlayGetHit());
        }
    }

    private System.Collections.IEnumerator PlayGetHit()
    {
        isHitReacting = true;               // 이동 일시 정지
        agent.isStopped = true;             // NavMesh 에이전트 멈춤

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
}
