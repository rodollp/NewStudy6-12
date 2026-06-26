using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField] private Transform player;

    [Header("실제 카메라")]
    [SerializeField] private Transform cameraTransform;

    [Header("카메라 위치 오프셋")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 8f, -8f);

    [Header("카메라가 바라볼 높이")]
    [SerializeField] private float lookHeight = 1f;

    [Header("카메라 따라가는 속도")]
    [SerializeField] private float followSpeed = 10f;

    private void LateUpdate()
    {
        if (player == null || cameraTransform == null)
            return;

        Vector3 targetPosition = player.position + offset;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position,targetPosition,followSpeed * Time.deltaTime);

        Vector3 lookPoint = player.position + Vector3.up * lookHeight;

        cameraTransform.LookAt(lookPoint);
    }
}