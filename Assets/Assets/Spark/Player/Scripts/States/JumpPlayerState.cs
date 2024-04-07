using System.Diagnostics;
using UnityEngine;

public class JumpPlayerState : PlayerState
{
    [SerializeField] private Animator anim;
    [SerializeField] private Player player;

    private void Start()
    {
        GameObject playerObject = GameObject.Find("Spark Player"); // Change "Player" to the name of your player GameObject
        player = playerObject.GetComponent<Player>();
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
                player.stats.lastPressedTime = Time.time;
                player.stats.numOfPresses++;
                player.attacking = true;
                player.ChangeBounds(2);

                if (player.stats.numOfPresses == 1)
                {
                    anim.SetBool("Attack1", true);
                }
                player.stats.numOfPresses = Mathf.Clamp(player.stats.numOfPresses, 0, player.stats.maxAirPresses);

            }

        }
        else
        {
            player.state.ChangeState<WalkPlayerState>();
        }
    }

    public override void Exit(Player player)
    {
        player.attacking = false;
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
    }

    public void return1()
    {
        if (player.stats.numOfPresses >= 2)
        {
            anim.SetBool("Attack2", true);
        }
        else
        {
            anim.SetBool("Attack1", false);
            player.stats.numOfPresses = 0;
            player.attacking = false;
            player.ChangeBounds(0);
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
            anim.SetBool("Attack2", false);
            anim.SetBool("Attack1", false);
            player.stats.numOfPresses = 0;
            player.attacking = false;
            player.ChangeBounds(0);
        }
    }

    public void return3()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        player.stats.numOfPresses = 0;
        player.attacking = false;
        player.ChangeBounds(0);
    }
}