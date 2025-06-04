using UnityEngine;

/*
 * CreateJoystick
 * PlayerInputProcess
 */
public class SurvivalController
    : PlayerController
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        if (!character)
            return;

        base.Update();
        EvaluateAxisKeyState();
    }

    protected void EvaluateAxisKeyState()
    {
        if (inputLock)
        {
            character.Movement.MoveToDirection(Vector2.zero);
            return;
        }

        Vector2 inputDirection;
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.y = Input.GetAxisRaw("Vertical");

        character.OnInputAxis(inputDirection);
    }
}
