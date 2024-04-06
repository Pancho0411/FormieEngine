using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerState : PlayerState
{
    public override void Enter(Player player)
    {

    }

    public override void Step(Player player, float deltaTime)
    {
        player.HandleGravity(deltaTime);
        player.HandleFall();
        if (player.input.jumpActionDown)
        {
            player.HandleJump();
        }
        else if (player.input.down)
        {
            player.state.ChangeState<CrouchPlayerState>();
        }
        else if (player.input.up)
        {
            player.state.ChangeState<LookUpPlayerState>();
        }
        else if (player.input.right || player.input.left)
        {
            player.state.ChangeState<WalkPlayerState>();
        }
    }

    public override void Exit(Player player)
    {
        player.rotation = player.originalRotation;
    }
}
