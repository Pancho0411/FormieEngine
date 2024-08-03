using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AirDashPlayerState : PlayerState
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

        //this kills your momentum like official Spark games. Recommended for the Air Dash
        player.velocity.x = player.stats.dashSpeed * player.direction;

        //this lets you keep your momentum and go faster. Can give you a really slow Air Dash
        //player.velocity.x += (player.stats.dashSpeed * player.direction);

        // Set isDashing flag to true when entering the dash state
        player.isDashing = true;

        player.particles.dashLines.Play();
    }
    
    public override void Step(Player player, float deltaTime)
    {
        //player.UpdateDirection(player.input.horizontal);
        player.HandleSlopeFactor(deltaTime);
        player.HandleAcceleration(deltaTime);
        player.HandleFriction(deltaTime);
        player.HandleGravity(deltaTime);
        player.HandleFall();
        player.camera.maxSpeed = 200;

        // Check if the player is grounded
        if(player.grounded)
        {
            player.state.ChangeState<WalkPlayerState>();
        }
        else if(player.input.jumpAction && player.input.vertical < 0)
        {
            player.state.ChangeState<DownDashPlayerState>();
        }

        // Clamp velocity to a maximum value
        player.velocity.x = Mathf.Clamp(player.velocity.x, -player.stats.maxSpeed, player.stats.maxSpeed);
    }

    public override void Exit(Player player)
    {
        player.StartCoroutine(InterpolateCameraSpeed(player.camera, camSpeed, camSpeedInterpolationDuration));
        player.isDashing = false;
        player.particles.dashLines.Stop();
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
}
