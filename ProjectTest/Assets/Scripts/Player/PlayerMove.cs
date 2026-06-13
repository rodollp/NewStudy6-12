using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runMultiplier = 2f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float checkLine = 0.1f;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Animator anim;

    private Rigidbody rb;

    private Vector2 inputVec;
    private bool jumpRequest;
    private bool isGround;
    private bool isRun;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        inputVec = Vector2.zero;

        if (Keyboard.current == null) return;

        float h = 0;
        float v = 0;

        if (Keyboard.current.aKey.isPressed) h = -1;
        if (Keyboard.current.dKey.isPressed) h = 1;
        if (Keyboard.current.sKey.isPressed) v = -1;
        if (Keyboard.current.wKey.isPressed) v = 1;

        inputVec = new Vector2(h, v);

        // Shift를 누르고 있으면 달리기
        isRun = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
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

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * inputVec.y + right * inputVec.x;

        // 대각선 이동이 더 빨라지지 않게 막음
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
            12f * Time.fixedDeltaTime
        );

        if (dir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                10f * Time.fixedDeltaTime
            );
        }

        if (jumpRequest && isGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            anim.SetTrigger("Jump");
        }

        jumpRequest = false;

        // Animator Speed 값
        // 0 = Idle, 0.5 = Walk, 1 = Run
        float animSpeed = 0f;

        if (dir.magnitude > 0.1f)
        {
            animSpeed = isRun ? 1f : 0.5f;
        }

        anim.SetFloat("Speed", animSpeed);
    }
}