using System.Collections;
using UnityEngine;

public class AttackPlayerState : PlayerState
{
    [SerializeField] private Player player;
    [SerializeField] private Animator anim;

    private bool initialPressRegistered = false;
    private bool wasGroundedAtStart = false;

    public override void Enter(Player player)
    {
        player.attacking = true;
        wasGroundedAtStart = player.grounded;

        // Check if the attack button is already pressed when entering the state
        if (player.input.attackActionDown)
        {
            player.stats.lastPressedTime = Time.time;
            player.stats.numOfPresses = 1;
            initialPressRegistered = true;
            anim.SetBool("Attack1", true);
        }
        else
        {
            player.stats.numOfPresses = 0;
            initialPressRegistered = false;
        }
    }

    public override void Step(Player player, float deltaTime)
    {
        player.UpdateDirection(player.input.horizontal);
        player.HandleSlopeFactor(deltaTime);
        player.HandleAcceleration(deltaTime);
        player.HandleFriction(deltaTime);
        player.HandleGravity(deltaTime);
        player.HandleFall();

        //updates hit box to face current direction
        if (!anim.GetBool("Direction"))
        {
            player.ChangeBounds(2);
        }
        else
        {
            player.ChangeBounds(3);
        }

        // Cancel attack if the player lands mid-attack
        if (!wasGroundedAtStart && player.grounded)
        {
            ResetAttackState();
            return;
        }

        // Check if the attack button is pressed during the step and register it
        if (player.input.attackActionDown)
        {
            if (!initialPressRegistered)
            {
                player.stats.lastPressedTime = Time.time;
                player.stats.numOfPresses++;
                player.stats.numOfPresses = Mathf.Clamp(player.stats.numOfPresses, 0, player.stats.maxPresses);

                // Update animation state based on number of presses
                SetAttackAnimation((int)player.stats.numOfPresses);
            }
            initialPressRegistered = true;
        }
        else
        {
            initialPressRegistered = false;
        }
    }

    public override void Exit(Player player)
    {
        player.attacking = false;
        player.ChangeBounds(0);
    }

    private void SetAttackAnimation(int numOfPresses)
    {
        for (int i = 1; i <= player.stats.maxPresses; i++)
        {
            anim.SetBool($"Attack{i}", i == numOfPresses);
        }
    }

    public void return1()
    {
        if (player.stats.numOfPresses >= 2)
        {
            anim.SetBool("Attack2", true);
        }
        else
        {
            ResetAttackState();
        }
    }

    public void return2()
    {
        if (player.stats.numOfPresses >= 3)
        {
            anim.SetBool("Attack3", true);
        }
        else
        {
            ResetAttackState();
        }
    }

    public void return3()
    {
        if (player.stats.numOfPresses >= 4)
        {
            anim.SetBool("Attack4", true);
        }
        else
        {
            ResetAttackState();
        }
    }

    public void return4()
    {
        if (player.stats.numOfPresses >= 5)
        {
            anim.SetBool("Attack5", true);
        }
        else
        {
            ResetAttackState();
        }
    }

    public void return5()
    {
        ResetAttackState();
    }

    private void ResetAttackState()
    {
        for (int i = 1; i <= player.stats.maxPresses; i++)
        {
            anim.SetBool($"Attack{i}", false);
        }
        player.stats.numOfPresses = 0;
        player.attacking = false;
        if (player.grounded)
        {
            player.state.ChangeState<WalkPlayerState>();
        }
        else
        {
            player.state.ChangeState<JumpPlayerState>();
        }
    }
}
