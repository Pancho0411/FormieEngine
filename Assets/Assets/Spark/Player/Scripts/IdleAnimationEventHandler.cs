using UnityEngine;

public class IdleAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private IdlePlayerState idlePlayerState;
    [SerializeField] private Player player;
    [SerializeField] private string playerName;

    void Start()
    {
        // Find the GameObject with the DashPlayerState script
        GameObject playerObject = GameObject.Find(playerName); // Change "Player" to the name of your player GameObject
        if (playerObject != null)
        {
            // Get the DashPlayerState component
            idlePlayerState = playerObject.GetComponent<IdlePlayerState>();
            player = playerObject.GetComponent<Player>();
        }
    }

    // Call the onDashFinish method when the dash animation finishes
    public void OnIdleAnimationFinish()
    {
        if (idlePlayerState != null)
        {
            idlePlayerState.stopIdling(player);
        }
    }
}
