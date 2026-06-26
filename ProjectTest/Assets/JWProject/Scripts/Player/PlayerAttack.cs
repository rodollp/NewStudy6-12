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
        //플레이어 위치에서 반지름 radius 만큼의 구로 attackDistance만큼의 거리상 플레이어 정면에 있는것이 없으면 리턴
        if (!Physics.SphereCast(transform.position, radius, transform.forward, out RaycastHit hit, attackDistance,monsterLayer))
            return;

        //맞았으면 이놈이 몬스터스테이터스를 가지고 있는지
        Debug.Log("공격에 맞은 오브젝트 : " + hit.collider.name);

        Monster monster = hit.collider.GetComponentInParent<Monster>();

        if (monster == null)
        {
            Debug.Log("Monster 컴포넌트를 찾지 못함");
            return;
        }

        monster.TakeDamage(playerStatus.Atk);

        MonsterAI monsterAI = monster.GetComponent<MonsterAI>();

        if (monsterAI != null)
        {
            monsterAI.OnHit();
        }

        Rigidbody rb = monster.GetComponent<Rigidbody>();

        KnockBack(rb);
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
        // 플레이어 정면
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);

        // SphereCast 범위
        Gizmos.color = Color.red;

        Vector3 endPos = transform.position + transform.forward * attackDistance;

        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.DrawWireSphere(endPos, radius);

        Gizmos.DrawLine(transform.position, endPos);
    }
}