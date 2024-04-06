using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DownDashPlayerState : PlayerState
{
    float camSpeed;
    public float camSpeedInterpolationDuration = 0.5f;

    public override void Enter(Player player)
    {
        player.ChangeBounds(0);
        player.PlayAudio(player.audios.dash, 1f);
        camSpeed = player.camera.maxSpeed;

        if (player.input.dashAction)
        {
            // Apply boost speed based on direction
            player.velocity.y = player.stats.dashSpeed;
        }
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
        if (player.grounded && player.input.dashAction)
        {
            player.state.ChangeState<DashPlayerState>();
        }
        else if (player.grounded && !player.input.dashAction)
        {
            player.state.ChangeState<WalkPlayerState>();
        }
        else if (!player.grounded && player.input.jumpAction && player.input.vertical < 0)
        {
            player.state.ChangeState<DownDashPlayerState>();
        }

        // Clamp velocity to a maximum value
        player.velocity.x = Mathf.Clamp(player.velocity.x, -player.stats.maxSpeed, player.stats.maxSpeed);
    }

    public override void Exit(Player player)
    {
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
}
