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
        base.Update();

        EvaluateAxisKeyState();
    }

    protected void EvaluateAxisKeyState()
    {
        Vector2 inputDirection;
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.y = Input.GetAxisRaw("Vertical");

        character.OnInputAxis(inputDirection);
    }
}
