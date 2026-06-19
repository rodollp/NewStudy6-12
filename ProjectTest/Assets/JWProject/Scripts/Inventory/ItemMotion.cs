using UnityEngine;

public class ItemMotion : MonoBehaviour
{
    [Header("위아래 움직임")]
    [SerializeField] float floatHeight = 0.3f;
    [SerializeField] float floatSpeed = 2f;

    [Header("회전")]
    [SerializeField] float rotateSpeed = 120f;

    Vector3 startPos;

    private void Start()
    {
        // 아이템이 생성된 처음 위치 저장
        startPos = transform.position;
    }

    private void Update()
    {
        // 위아래로 부드럽게 움직임
        float y = Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = startPos + new Vector3(0, y, 0);

        // Y축 기준으로 빙글빙글 회전
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
