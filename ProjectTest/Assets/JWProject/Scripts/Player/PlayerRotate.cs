using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform visual;
    [SerializeField] private float rotateSpeed;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 dir = hit.point - transform.position;
            dir.y = 0;

            Debug.DrawRay(transform.position,dir.normalized*3f, Color.red);

            Quaternion targetRot = Quaternion.LookRotation(dir);

            visual.rotation = Quaternion.Slerp(
                visual.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }

        
    }
}