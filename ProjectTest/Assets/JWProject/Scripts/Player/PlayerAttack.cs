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
        if (!Physics.SphereCast(transform.position, radius, transform.forward, out RaycastHit hit, attackDistance))
            return;

        //맞았으면 이놈이 몬스터스테이터스를 가지고 있는지
        Monster monster = hit.transform.GetComponent<Monster>();

        if (monster == null)
            return;
        monster.TakeDamage(playerStatus.Atk);

        Rigidbody rb = hit.transform.GetComponent<Rigidbody>();

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