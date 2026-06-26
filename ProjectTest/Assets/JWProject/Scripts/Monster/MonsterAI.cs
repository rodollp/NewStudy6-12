using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] protected Transform player;

    [Header("몬스터 눈 위치")]
    [SerializeField] protected Transform eyePoint;

    [Header("NavMesh")]
    [SerializeField] protected NavMeshAgent agent;

    [Header("이동 속도")]
    [SerializeField] protected float moveSpeed = 3f;

    [Header("시야각")]
    [SerializeField] protected float sightAngle = 60f;

    [Header("인식 범위")]
    [SerializeField] protected float detectRange = 10f;

    [Header("공격 범위")]
    [SerializeField] protected float attackRange = 2f;

    protected float attackTimer = 0f;
    protected float coolDown = 1f;

    private bool isAggro = false;

    protected enum MonsterState
    {
        Idle,
        Chase,
        Attack
    }

    protected MonsterState currentState = MonsterState.Idle;

    protected virtual void Awake()
    {
        PlayerStatus playerStatus = FindAnyObjectByType<PlayerStatus>();

        if (playerStatus != null)
        {
            player = playerStatus.transform;
        }

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (agent != null)
        {
            agent.speed = moveSpeed;

            // Chase 회전은 NavMeshAgent가 담당
            agent.updateRotation = true;
        }
    }

    protected virtual void Update()
    {
        if (player == null)
            return;

        switch (currentState)
        {
            case MonsterState.Idle:
                Idle();
                break;

            case MonsterState.Chase:
                Chase();
                break;

            case MonsterState.Attack:
                Attack();
                break;
        }
    }

    protected virtual bool CanSeePlayer()
    {
        if (eyePoint == null)
            return false;

        Vector3 toPlayer = (player.position - eyePoint.position).normalized;

        float distance = (player.position - transform.position).sqrMagnitude;

        if (distance > detectRange * detectRange)
            return false;

        float dot = Vector3.Dot(eyePoint.forward, toPlayer);

        float limitDot = Mathf.Cos(sightAngle * 0.5f * Mathf.Deg2Rad);

        return dot >= limitDot;
    }

    protected virtual bool IsInAttackRange()
    {
        float distance = (player.position - transform.position).sqrMagnitude;

        return distance < attackRange * attackRange;
    }

    protected virtual void MoveToPlayer()
    {
        if (agent == null)
            return;

        if (!agent.isOnNavMesh)
            return;

        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    protected virtual void StopAgent()
    {
        if (agent == null)
            return;

        if (!agent.isOnNavMesh)
            return;

        agent.isStopped = true;
        agent.ResetPath();
    }

    protected virtual void Idle()
    {
        StopAgent();

        if (CanSeePlayer() || isAggro)
        {
            currentState = MonsterState.Chase;
        }
    }
    protected virtual void Chase()
    {
        // 인식 범위 밖으로 나가면 추적 종료
        if (!IsInDetectRange())
        {
            isAggro = false;
            StopAgent();
            currentState = MonsterState.Idle;
            return;
        }

        if (IsInAttackRange())
        {
            StopAgent();
            currentState = MonsterState.Attack;
            return;
        }

        MoveToPlayer();
    }
    protected virtual bool IsInDetectRange()
    {
        float distance = (player.position - transform.position).sqrMagnitude;

        return distance <= detectRange * detectRange;
    }
    protected virtual void Attack()
    {
        StopAgent();

        MonsterAttack();

        float exitAttackRange = attackRange + 1f;
        float distance = (player.position - transform.position).sqrMagnitude;

        if (distance > exitAttackRange * exitAttackRange)
        {
            currentState = MonsterState.Chase;
        }
    }

    protected virtual void MonsterAttack()
    {
        Monster status = GetComponent<Monster>();

        attackTimer += Time.deltaTime;

        if (attackTimer >= coolDown)
        {
            attackTimer = 0f;

            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();

            if (playerStatus != null && status != null)
            {
                playerStatus.TakeDamage(status.Atk);
            }
        }
    }

    public virtual void OnHit()
    {
        isAggro = true;
        currentState = MonsterState.Chase;
    }

    protected void OnDrawGizmos()
    {
        if (eyePoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector3 leftDir =
            Quaternion.Euler(0, -sightAngle * 0.5f, 0) * eyePoint.forward;

        Vector3 rightDir =
            Quaternion.Euler(0, sightAngle * 0.5f, 0) * eyePoint.forward;

        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(eyePoint.position, eyePoint.position + leftDir * detectRange);
        Gizmos.DrawLine(eyePoint.position, eyePoint.position + rightDir * detectRange);

        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(eyePoint.position, player.position);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(eyePoint.position, eyePoint.position + eyePoint.forward * detectRange);
    }
}