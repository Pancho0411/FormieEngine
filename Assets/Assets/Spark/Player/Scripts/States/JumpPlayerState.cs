using System.Diagnostics;
using UnityEngine;

public class JumpPlayerState : PlayerState
{
    private void Start()
    {

    }

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
            else if (player.input.dashAction && player.input.vertical < 0 && player.input.jumpAction)
            {
                player.state.ChangeState<DownDashPlayerState>();
            }
            else if (player.input.dashAction && player.input.horizontal != 0)
            {
                player.state.ChangeState<AirDashPlayerState>();
            }
            else if (player.input.attackActionDown)
            {
                player.state.ChangeState<AttackPlayerState>();
            }
        }
        else
        {
            player.state.ChangeState<WalkPlayerState>();
        }
    }

    public override void Exit(Player player)
    {

    }
}