using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTarget : MonoBehaviour
{
    [Header("카메라 기준 타겟 (플레이어 머리)")]
    [SerializeField] Transform cameraTarget;
    [Header("플레이어 설정")]
    [SerializeField] Transform player;

    [Header("실제 카메라 Transform")]
    [SerializeField] Transform cameraTransform;

    [Header("카메라 감도 (작게 시작 추천 0.02~0.05)")]
    [SerializeField] float mouseSensitivity = 0.03f;

    [Header("카메라 거리 (뒤로 얼마나 떨어질지)")]
    [SerializeField] float distance = 3f;

    [Header("카메라 높이 (머리 기준 위로 얼마나 올라갈지)")]
    [SerializeField] float height = 1.5f;

    // Yaw = 좌우 회전 (수평 회전)
    float turnLR;

    // Pitch = 상하 회전 (위/아래)
    float turnUpDown;

    private void Start()
    {
        // 마우스 커서 고정
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        // ===== 1. 마우스 입력 읽기 =====
        if (Mouse.current != null)
        {
            // 마우스 이동량 (프레임 기준)
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            // 좌우 회전
            turnLR += mouseDelta.x * mouseSensitivity;

            // 상하 회전
            turnUpDown -= mouseDelta.y * mouseSensitivity;

            // 위아래 각도 제한 (고개 꺾임 방지)
            turnUpDown = Mathf.Clamp(turnUpDown, -35f, 60f);
        }

        // ===== 2. 회전값 생성 =====
        Quaternion rotation = Quaternion.Euler(turnUpDown, turnLR, 0);
        player.rotation = Quaternion.Euler(0,turnLR,0);

        // ===== 3. 카메라 뒤로 밀기 =====
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        // ===== 4. 카메라 기준 중심점 =====
        Vector3 focusPoint = cameraTarget.position + Vector3.up * height;

        // ===== 5. 최종 카메라 위치 설정 =====
        cameraTransform.position = focusPoint + offset;

        // ===== 6. 항상 타겟을 바라보게 설정 =====
        cameraTransform.LookAt(focusPoint);

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, focusPoint + offset, 10f * Time.deltaTime);



    }
}