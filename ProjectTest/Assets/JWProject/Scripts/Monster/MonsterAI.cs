using System;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] protected Transform player;

    [Header("몬스터 눈 위치")]
    [SerializeField] protected Transform eyePoint;

    [Header("이동 속도")]
    [SerializeField] float moveSpeed = 3f;

    [Header("시야각")]
    [SerializeField] float sightAngle = 60f;

    [Header("인식 범위")]
    [SerializeField] float detectRange = 10f;

    [Header("공격 범위")]
    [SerializeField] float attackRange = 2f;
    float attackTimer = 0;
    float coolDown = 1f;
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
    }
    protected virtual void Update()
    {
        

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
        Vector3 toPlayer =(player.position - eyePoint.position).normalized;

        float distance =(player.position - transform.position).sqrMagnitude;

        if (distance > detectRange * detectRange)
            return false;

        float dot =Vector3.Dot(eyePoint.forward, toPlayer);

        float limitDot =Mathf.Cos(sightAngle * 0.5f * Mathf.Deg2Rad);

        return dot >= limitDot;
    }



    protected virtual bool IsInAttackRange()
    {
        float distance =(player.position - transform.position).sqrMagnitude;

        return distance < attackRange * attackRange;
    }
    protected virtual void MoveToPlayer()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    protected virtual void LookPlayer()
    {
        Vector3 dir =player.position - transform.position;

        dir.y = 0;

        if(dir == Vector3.zero) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
    }
    protected virtual void Idle()
    {

        if (CanSeePlayer())
        {
            currentState = MonsterState.Chase;
        }
    }
    protected virtual void Chase()
    {
        LookPlayer();

        if (IsInAttackRange())
        {
            currentState = MonsterState.Attack;
            return;
        }

        MoveToPlayer();

        if (!CanSeePlayer())
        {
            currentState = MonsterState.Idle;
        }
    }

    protected virtual void Attack()
    {
        LookPlayer();

        MonsterAttack();

        float exitAttackRange = attackRange + 1f;

        float distance =(player.position - transform.position).sqrMagnitude;

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
            if (playerStatus != null)
            {
                playerStatus.TakeDamage(status.Atk);
            }
        }
    }
    protected void OnDrawGizmos()
    {
        if (eyePoint == null)
            return;

        // =========================
        // 인식 범위
        // =========================

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // =========================
        // 공격 범위
        // =========================

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // =========================
        // 시야각 좌/우 선
        // =========================

        Vector3 leftDir =Quaternion.Euler(0, -sightAngle * 0.5f, 0) * eyePoint.forward;

        Vector3 rightDir =Quaternion.Euler(0, sightAngle * 0.5f, 0) * eyePoint.forward;

        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(eyePoint.position, eyePoint.position + leftDir * detectRange);

        Gizmos.DrawLine(eyePoint.position, eyePoint.position + rightDir * detectRange);

        // =========================
        // 플레이어 방향 확인
        // =========================

        if (player != null)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(eyePoint.position, player.position);
        }

        // =========================
        // 몬스터 정면
        // =========================

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(eyePoint.position, eyePoint.position + eyePoint.forward * detectRange);
    }
}