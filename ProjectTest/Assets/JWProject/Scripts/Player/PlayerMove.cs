using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runMultiplier = 2f;
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float checkLine = 0.1f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerInputHandler input;
    

    private Rigidbody rb;

    private Vector2 inputVec;
    private bool jumpRequest;
    private bool isGround;
    private bool isRun;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (input == null)
            input = GetComponent<PlayerInputHandler>();

        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        inputVec = input.MoveInput;
        isRun = input.IsRun;

        if (input.JumpPressed)
        {
            jumpRequest = true;
        }
    }

    private void FixedUpdate()
    {
        isGround = Physics.Raycast(
            groundCheck.position,
            Vector3.down,
            checkLine
        );

        Debug.DrawRay(
            groundCheck.position,
            Vector3.down * checkLine,
            isGround ? Color.green : Color.red
        );

        // 쿼터뷰 고정 카메라 기준: 월드 X/Z 방향 이동
        Vector3 dir = new Vector3(inputVec.x, 0f, inputVec.y);

        if (dir.magnitude > 1f)
            dir.Normalize();

        float currentSpeed = isRun ? moveSpeed * runMultiplier : moveSpeed;

        Vector3 targetVelocity = new Vector3(
            dir.x * currentSpeed,
            rb.linearVelocity.y,
            dir.z * currentSpeed
        );

        rb.linearVelocity = Vector3.Lerp(
            rb.linearVelocity,
            targetVelocity,
            acceleration * Time.fixedDeltaTime
        );


        if (jumpRequest && isGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }

        jumpRequest = false;

        float animSpeed = 0f;

        if (dir.magnitude > 0.1f)
        {
            animSpeed = isRun ? 1f : 0.5f;
        }

        anim.SetFloat("Speed", animSpeed);
    }
}