using UnityEngine;

public class DashAnimationEventHandler : MonoBehaviour
{
    [SerializeField]private DashPlayerState dashPlayerState;
    [SerializeField] private Player player;

    void Start()
    {
        // Find the GameObject with the DashPlayerState script
        GameObject playerObject = GameObject.Find("Spark Player"); // Change "Player" to the name of your player GameObject
        if (playerObject != null)
        {
            // Get the DashPlayerState component
            dashPlayerState = playerObject.GetComponent<DashPlayerState>();
            player = playerObject.GetComponent<Player>();
        }
    }

    // Call the onDashFinish method when the dash animation finishes
    public void OnDashAnimationFinish()
    {
        if (dashPlayerState != null)
        {
            dashPlayerState.onDashFinish(player);
        }
    }
}
