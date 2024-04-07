using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WalkPlayerState : PlayerState
{
    [SerializeField] private Animator anim;
    [SerializeField] private Player player;


    private float orgNoP;

    private void Start()
    {
        GameObject playerObject = GameObject.Find("Spark Player"); // Change "Player" to the name of your player GameObject
        player = playerObject.GetComponent<Player>();
    }

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
        if (player.input.attackActionDown && player.grounded)
        {
            player.stats.lastPressedTime = Time.time;
            player.stats.numOfPresses++;
            player.attacking = true;
            player.ChangeBounds(2);

            if (player.stats.numOfPresses == 1)
            {
                anim.SetBool("Attack1", true);
            }
            player.stats.numOfPresses = Mathf.Clamp(player.stats.numOfPresses, 0, player.stats.maxPresses);
        }
    }

    public override void Exit(Player player)
    {
        player.attacking = false;
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        anim.SetBool("Attack4", false);
        anim.SetBool("Attack5", false);
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
        if (player.stats.numOfPresses >= 4)
        {
            anim.SetBool("Attack4", true);
        }
        else
        {
            anim.SetBool("Attack3", false);
            anim.SetBool("Attack2", false);
            anim.SetBool("Attack1", false);
            player.stats.numOfPresses = 0;
            player.attacking = false;
            player.ChangeBounds(0);
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
            anim.SetBool("Attack4", false);
            anim.SetBool("Attack3", false);
            anim.SetBool("Attack2", false);
            anim.SetBool("Attack1", false);
            player.stats.numOfPresses = 0;
            player.attacking = false;
            player.ChangeBounds(0);
        }
    }

    public void return5()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        anim.SetBool("Attack4", false);
        anim.SetBool("Attack5", false);
        player.stats.numOfPresses = 0;
        player.attacking = false;
        player.ChangeBounds(0);
    }
}