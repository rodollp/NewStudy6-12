using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }

    public bool IsRun { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool UsePotionPressed { get; private set; }
    public bool UseWeaponPressed { get; private set; }
    public bool NextStagePressed { get; private set; }

    private void Update()
    {
        ResetOneFrameInputs();

        if (Keyboard.current == null)
            return;

        float h = 0f;
        float v = 0f;

        if (Keyboard.current.aKey.isPressed) h = -1f;
        if (Keyboard.current.dKey.isPressed) h = 1f;
        if (Keyboard.current.sKey.isPressed) v = -1f;
        if (Keyboard.current.wKey.isPressed) v = 1f;

        MoveInput = new Vector2(h, v);

        if (MoveInput.magnitude > 1f)
            MoveInput = MoveInput.normalized;

        IsRun =Keyboard.current.leftShiftKey.isPressed ||Keyboard.current.rightShiftKey.isPressed;

        JumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;

        if (Mouse.current != null)
            AttackPressed = Mouse.current.leftButton.wasPressedThisFrame;

        InteractPressed = Keyboard.current.gKey.wasPressedThisFrame;
        UsePotionPressed = Keyboard.current.hKey.wasPressedThisFrame;
        UseWeaponPressed = Keyboard.current.pKey.wasPressedThisFrame;
        NextStagePressed = Keyboard.current.nKey.wasPressedThisFrame;
    }

    private void ResetOneFrameInputs()
    {
        JumpPressed = false;
        AttackPressed = false;
        InteractPressed = false;
        UsePotionPressed = false;
        UseWeaponPressed = false;
        NextStagePressed = false;
    }
}