using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerState : PlayerState
{

    [SerializeField] private Animator anim;

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
        else if (player.input.right || player.input.left || player.input.attackActionDown)
        {
            player.state.ChangeState<WalkPlayerState>();
        }
    }

    public override void Exit(Player player)
    {

    }

    public void stopIdling(Player player)
    {
        player.state.ChangeState<WalkPlayerState> ();
    }
}
