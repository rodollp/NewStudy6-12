using UnityEngine;
using UnityEngine.AI;

public class Navi : MonoBehaviour
{
    [SerializeField] Transform target;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
