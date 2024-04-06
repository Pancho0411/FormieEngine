using System.Diagnostics;
using UnityEngine;

public class JumpPlayerState : PlayerState
{
    public override void Enter(Player player)
    {
        player.ChangeBounds(0);
    }

    public override void Step(Player player, float deltaTime)
    {
        player.UpdateDirection(player.input.horizontal);
        player.HandleAcceleration(deltaTime);
        player.HandleGravity(deltaTime);

        if (!player.grounded)
        {
            if (player.input.jumpActionUp && player.velocity.y > player.stats.minJumpHeight)
            {
                player.velocity.y = player.stats.minJumpHeight;
            }
            else if (player.input.dashAction)
            {
                player.state.ChangeState<AirDashPlayerState>();
            }
            else if (player.input.dashAction && player.input.vertical < 0)
            {
                player.state.ChangeState<DownDashPlayerState>();
            }
        }
        else
        {
            player.state.ChangeState<WalkPlayerState>();
        }
    }

    public override void Exit(Player player)
    {
        player.rotation = player.originalRotation;
    }
}