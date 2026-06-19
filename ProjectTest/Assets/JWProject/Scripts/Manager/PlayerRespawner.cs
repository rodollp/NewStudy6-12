using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform startPoint;

    public void Respawn()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = startPoint.position;
        rb.rotation = startPoint.rotation;

        PlayerStatus status = player.GetComponent<PlayerStatus>();
        status.FullHeal();
    }
}