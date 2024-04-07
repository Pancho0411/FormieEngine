using UnityEngine;
using System.Collections;

public class DashPlayerState : PlayerState
{
    float camSpeed;
    public float camSpeedInterpolationDuration = 0.5f;
    private int curDirection;

    public override void Enter(Player player)
    {
        player.ChangeBounds(0);
        player.PlayAudio(player.audios.dash, 1f);
        camSpeed = player.camera.maxSpeed;
        curDirection = player.direction;
        player.attacking = false;

        player.velocity.x = player.stats.dashSpeed * player.direction;

        // Set isDashing flag to true when entering the dash state
        player.isDashing = true;
    }

    public override void Step(Player player, float deltaTime)
    {
        player.UpdateDirection(player.input.horizontal);
        player.HandleSlopeFactor(deltaTime);
        player.HandleAcceleration(deltaTime);
        player.HandleFriction(deltaTime);
        player.HandleGravity(deltaTime);
        player.HandleFall();
        player.camera.maxSpeed = 200;

        // Check if the player is grounded
        if (player.grounded && player.isDashing)
        {
            // Check for other actions like jumping
            if (player.input.jumpActionDown)
            {
                player.HandleJump();
            }
            //if you stop or switch direction, go back to walk state
            else if (player.velocity.x == 0 || player.direction != curDirection)
            {
                player.state.ChangeState<WalkPlayerState>();
            }
            else if (player.input.jumpActionDown && player.input.dashActionDown)
            {
                player.HandleJump();
            }
        }
        else if (player.grounded && player.input.dashActionDown)
        {
            player.state.ChangeState<WalkPlayerState>(); // Transition to walk state
        }

        // Clamp velocity to a maximum value
        player.velocity.x = Mathf.Clamp(player.velocity.x, -player.stats.maxSpeed, player.stats.maxSpeed);
    }


    public override void Exit(Player player)
    {
        player.isDashing = false;
        player.StartCoroutine(InterpolateCameraSpeed(player.camera, camSpeed, camSpeedInterpolationDuration));
    }

    private IEnumerator InterpolateCameraSpeed(PlayerCamera camera, float targetSpeed, float duration)
    {
        float elapsedTime = 0;
        float startSpeed = camera.maxSpeed;

        // Interpolate the camera speed over the specified duration
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            camera.maxSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / duration);
            yield return null;
        }

        // Ensure the camera speed is set to the target speed at the end
        camera.maxSpeed = targetSpeed;
    }

    public void onDashFinish(Player player)
    {
        player.isDashing = false;
        if(player.input.horizontal == 0)
        {
            player.velocity.x = Mathf.Lerp(player.velocity.x, 0f, 4f * Time.deltaTime);
        }
    }
}
