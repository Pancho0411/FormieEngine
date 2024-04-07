using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[System.Serializable]
public class PlayerInput
{
    [SerializeField] private string horizontalName = "Horizontal";
    [SerializeField] private string verticalName = "Vertical";
    [SerializeField] private string jumpActionName = "JumpAction";
    [SerializeField] private string attackName = "Attack";
    [SerializeField] private string dashName = "Dash";

    public float horizontal { get; private set; }
    public float vertical { get; private set; }

    public bool right { get; private set; }
    public bool left { get; private set; }
    public bool up { get; private set; }
    public bool down { get; private set; }

    public bool jumpAction { get; private set; }
    public bool jumpActionDown { get; private set; }
    public bool jumpActionUp { get; private set; }

    public bool dashAction { get; private set; }
    public bool dashActionDown { get; private set; }
    public bool dashActionUp { get; private set; }

    public bool attackAction { get; private set; }
    public bool attackActionDown { get; private set; }
    public bool attackActionUp { get; private set; }

    private bool controlLocked;
    private float unlockTimer;

    public void InputUpdate()
    {
        UpdateAxes();
        UpdateJumpAction();
        UpdateDashAction();
        UpdateAttackAction();
    }

    private void UpdateAxes()
    {
        horizontal = !controlLocked ? CrossPlatformInputManager.GetAxis(horizontalName) : 0;
        vertical = CrossPlatformInputManager.GetAxis(verticalName);
        jumpActionDown = jumpActionUp = false;
        right = horizontal > 0;
        left = horizontal < 0;
        up = vertical > 0;
        down = vertical < 0;
    }

    private void UpdateJumpAction()
    {
        if (CrossPlatformInputManager.GetButton(jumpActionName))
        {
            if (!jumpAction)
            {
                jumpAction = true;
                jumpActionDown = true;
            }
        }
        else
        {
            if (jumpAction)
            {
                jumpAction = false;
                jumpActionUp = true;
            }
        }
    }

    private void UpdateDashAction()
    {
        if (CrossPlatformInputManager.GetAxisRaw(dashName) > 0)
        {
            //Debug.Log("Dash pressed");
            if (!dashAction)
            {
                dashAction = true;
                dashActionDown = true;
            }
        }
        else
        {
            if (dashActionDown)
            {
                dashAction = false;
                dashActionUp = true;
            }
        }
    }

    private void UpdateAttackAction()
    {
        if (CrossPlatformInputManager.GetButton(attackName))
        {
            if (!attackAction)
            {
                attackAction = true;
                attackActionDown = true;
                attackActionUp = false;
            }
        }
        else
        {
            if (attackAction)
            {
                attackAction = false;
                attackActionUp = true;
                attackActionDown = false;
            }
        }
    }

    public void LockHorizontalControl(float time)
    {
        unlockTimer = time;
        controlLocked = true;
    }

    public void UnlockHorizontalControl(float deltaTime)
    {
        if (unlockTimer > 0)
        {
            unlockTimer -= deltaTime;

            if (unlockTimer <= 0)
            {
                unlockTimer = 0;
                controlLocked = false;
            }
        }
    }
}