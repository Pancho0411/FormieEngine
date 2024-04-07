public class HurtPlayerState : PlayerState
{
    public override void Enter(Player player)
    {
        player.GroundExit();
        player.ChangeBounds(0);
        player.invincible = true;
        player.halfGravity = true;
        player.attacking = false;
    }

    public override void Step(Player player, float deltaTime)
    {
        player.HandleGravity(deltaTime);

        if (player.grounded)
        {
            player.state.ChangeState<WalkPlayerState>();
        }
        else
        {
            //jump to recover
            if(player.input.jumpActionUp)
            {
                player.HandleJump();
            }
            //air dash to recover
            else if (player.input.dashAction)
            {
                player.state.ChangeState<AirDashPlayerState>();
            }
        }
    }

    public override void Exit(Player player)
    {
        player.halfGravity = false;
        player.skin.StartBlinking(player.stats.invincibleTime);
        player.invincibleTimer = player.stats.invincibleTime;
    }
}
