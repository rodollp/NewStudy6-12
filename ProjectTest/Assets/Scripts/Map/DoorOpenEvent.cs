using UnityEngine;
using UnityEngine.InputSystem;

public class DoorOpenEvent : MonoBehaviour
{
    [SerializeField] GameObject doorBody;

    bool canOpen;

    private void Update()
    {
        if (!canOpen) return;

        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            doorBody.SetActive(false);

            Invoke(nameof(CloseDoor), 2f);
        }
    }

    void CloseDoor()
    {
        doorBody.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        canOpen = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        canOpen = false;
    }
}