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

    //public AudioClip screamClip;       // scream.mp3 지정용
    //private AudioSource audioSource;

    // 비명 낼 확률 (0~1 사이)
    //[Range(0f, 1f)]
    //public float screamChance = 0.1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();      // 에이전트 컴포넌트 가져오기
        animator = GetComponent<Animator>();       // 애니메이터 가져오기
        //audioSource = GetComponent<AudioSource>(); // 사운드
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
            stuckTimer = 0f;
            return;
        }

        // 1초 이상 멈춰 있으면 경로 재설정
        if (!agent.pathPending && agent.hasPath && agent.velocity.sqrMagnitude < 1f)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= 1f)
            {
                Debug.Log("1초 이상 멈춤: 경로 재설정");
                SetRandomDestination();
                stuckTimer = 0f;
                timer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        // 랜덤하게 비명 시도
        //TryScreamRandomly();
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

    /*void TryScreamRandomly()
    {
        if (audioSource != null && screamClip != null && !audioSource.isPlaying)
        {
            if (Random.value < screamChance * Time.deltaTime * 60f)
            {
                audioSource.PlayOneShot(screamClip);
                Debug.Log("NPC가 비명을 질렀습니다!");
            }
        }
    }
    */
}
