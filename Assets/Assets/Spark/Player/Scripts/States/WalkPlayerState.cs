using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WalkPlayerState : PlayerState
{
    private float orgNoP;

    public override void Enter(Player player)
    {
        //player.attacking = false;
        player.ChangeBounds(0);
        player.ChangeBounds(0);
    }

    public override void Step(Player player, float deltaTime)
    {
        player.UpdateDirection(player.input.horizontal);
        player.HandleSlopeFactor(deltaTime);
        player.HandleAcceleration(deltaTime);
        player.HandleFriction(deltaTime);
        player.HandleGravity(deltaTime);
        player.HandleFall();

        if (player.grounded)
        {
            if (Time.time - player.stats.lastPressedTime > player.stats.cooldownTime)
            {
                player.stats.numOfPresses = 0;
            }
            if (player.input.jumpActionDown)
            {
                player.HandleJump();
            }
            else if (player.input.down && player.velocity.x == 0)
            {
                if (player.angle < player.stats.minAngleToSlide)
                {
                    player.state.ChangeState<CrouchPlayerState>();
                }
            }
            else if (player.input.up && player.velocity.x == 0)
            {
                player.state.ChangeState<LookUpPlayerState>();
            }
            else if (Mathf.Sign(player.velocity.x) != Mathf.Sign(player.input.horizontal) && player.input.horizontal != 0)
            {
                if (Mathf.Abs(player.velocity.x) >= player.stats.minSpeedToBrake)
                {
                    player.state.ChangeState<BrakePlayerState>();
                }
                else
                {
                    if (player.velocity.x > 0)
                    {
                        player.velocity.x = -player.stats.turnSpeed;
                    }
                    else
                    {
                        player.velocity.x = player.stats.turnSpeed;
                    }
                }
            }
            else if (player.input.dashAction)
            {
                player.state.ChangeState<DashPlayerState>();
            }
        }
        if (player.input.attackActionDown)
        {
            player.state.ChangeState<AttackPlayerState>();
        }
    }

    public override void Exit(Player player)
    {

    }

    
}