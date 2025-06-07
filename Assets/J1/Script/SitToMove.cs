using UnityEngine;
using UnityEngine.AI;


public class SitToMove : MonoBehaviour
{
    public Transform chairAnchor;                  // 의자 기준 위치
    public string sitAnimationName = "SitIdle";    // 앉기 애니메이션 이름
    public string standTriggerName = "StandUp";    // 일어나기 트리거 이름
    public float sitDuration = 5f;                 // 앉아있는 시간 (초)

    private Animator animator;
    private NavMeshAgent agent;
    private RandomPatrol patrol;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        patrol = GetComponent<RandomPatrol>();

        // NPC를 의자 위치로 이동시킴
        transform.position = chairAnchor.position;
        transform.rotation = chairAnchor.rotation;

        // 이동 기능 비활성화
        agent.enabled = false;
        patrol.enabled = false;

        // 앉기 애니메이션 실행
        animator.Play(sitAnimationName);

        // 지정 시간 뒤에 행동 시작
        Invoke(nameof(BeginEvacuation), sitDuration);
    }

    void BeginEvacuation()
    {
        // 일어나기 애니메이션 트리거
        animator.SetTrigger(standTriggerName);

        // 일어나고 1.5초 후 이동 시작 (애니메이션 길이에 따라 조정)
        Invoke(nameof(StartMoving), 1.5f);
    }

    void StartMoving()
    {
        agent.enabled = true;
        patrol.enabled = true;
    }
}
