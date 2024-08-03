using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class DownDashPlayerState : PlayerState
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

        player.velocity.y -= player.stats.downDashSpeed;
        player.velocity.x = 0;

        // Set isDashing flag to true when entering the dash state
        player.isDashing = true;

        player.particles.downDashLines.Play();
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
        if (player.grounded)
        {
            player.state.ChangeState<WalkPlayerState>();
        }

        // Clamp velocity to a maximum value
        player.velocity.y = Mathf.Clamp(player.velocity.y, -player.stats.maxSpeed, player.stats.maxSpeed);
    }

    public override void Exit(Player player)
    {
        player.StartCoroutine(InterpolateCameraSpeed(player.camera, camSpeed, camSpeedInterpolationDuration));
        player.isDashing = false;
        player.particles.downDashLines.Stop();
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
