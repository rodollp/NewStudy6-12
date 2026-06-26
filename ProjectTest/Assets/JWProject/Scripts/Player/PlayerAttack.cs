using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격 범위 설정")]
    [SerializeField] private float attackDistance = 1f;
    [Header("밀치는 힘")]
    [SerializeField] private float attackForce = 5f;
    [Header("구의 반지름")]
    [SerializeField] private float radius = 0.5f;
    [SerializeField] Animator anim;
    [SerializeField] private PlayerInputHandler input;

    [SerializeField]private LayerMask monsterLayer;

    PlayerStatus playerStatus;

    private void Awake()
    {
        playerStatus = GetComponent<PlayerStatus>();
        if (input == null) input = GetComponent<PlayerInputHandler>();
    }
    private void Start()
    {
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {


        if (input.AttackPressed)
        {
            CommonAttack();
        }


    }



    private void CommonAttack()
    {
        anim.SetTrigger("Attack");

        Vector3 start = transform.position + Vector3.up * 0.8f;
        Vector3 end = start + transform.forward * attackDistance;

        Collider[] hits = Physics.OverlapCapsule(
            start,
            end,
            radius,
            monsterLayer
        );

        Debug.Log("공격 범위 안 콜라이더 수 : " + hits.Length);

        HashSet<Monster> damagedMonsters = new HashSet<Monster>();

        foreach (Collider hit in hits)
        {
            Monster monster = hit.GetComponentInParent<Monster>();

            if (monster == null)
                continue;

            // Body, Head 둘 다 맞아서 중복 데미지 들어가는 것 방지
            if (damagedMonsters.Contains(monster))
                continue;

            damagedMonsters.Add(monster);

            monster.TakeDamage(playerStatus.Atk);

            MonsterAI monsterAI = monster.GetComponentInParent<MonsterAI>();

            if (monsterAI != null)
            {
                monsterAI.OnHit();
            }

            // NavMeshAgent 쓰는 중이면 일단 넉백은 잠깐 끄고 테스트 추천
            // Rigidbody rb = monster.GetComponentInParent<Rigidbody>();
            // KnockBack(rb);
        }
    }

    private void KnockBack(Rigidbody rb)
    {
        if (rb == null)
            return;

        Vector3 dir = (rb.transform.position - transform.position).normalized;

        Vector3 push = dir * attackForce;
        push.y = 1f;

        rb.AddForce(push, ForceMode.Impulse);
    }





    private void OnDrawGizmos()
    {
        Vector3 start = transform.position + Vector3.up * 0.8f;
        Vector3 end = start + transform.forward * attackDistance;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(start, end);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);
    }
}