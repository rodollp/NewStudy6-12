using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float checkLine = 0.1f;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform cameraTransform;

    private Rigidbody rb;

    private Vector2 inputVec;
    private bool jumpRequest;
    private bool isGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // ===== 입력 =====
        inputVec = Vector2.zero;

        if (Keyboard.current != null)
        {
            float h = 0;
            float v = 0;

            if (Keyboard.current.aKey.isPressed) h = -1;
            if (Keyboard.current.dKey.isPressed) h = 1;
            if (Keyboard.current.sKey.isPressed) v = -1;
            if (Keyboard.current.wKey.isPressed) v = 1;

            inputVec = new Vector2(h, v);

            // 점프 요청만 저장


            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                jumpRequest = true;
            }
        }
    }

    private void FixedUpdate()
    {
        isGround = Physics.Raycast(groundCheck.position, Vector3.down, checkLine);
        Debug.DrawRay(groundCheck.position, Vector3.down * checkLine, isGround ? Color.green : Color.red);
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * inputVec.y + right * inputVec.x;

        // ===== 이동 =====
        Vector3 targetVelocity = new Vector3(dir.x * moveSpeed, rb.linearVelocity.y,dir.z * moveSpeed);

        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity,targetVelocity,12f * Time.fixedDeltaTime);

        if (dir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.fixedDeltaTime);
        }

        // ===== 점프 =====
        if (jumpRequest && isGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequest = false;
        }
    }
}