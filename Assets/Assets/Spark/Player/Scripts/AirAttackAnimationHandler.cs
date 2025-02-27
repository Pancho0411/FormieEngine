using UnityEngine;

public class AirAttackAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private AttackPlayerState jumpPlayerState;
    [SerializeField] private Player player;
    [SerializeField] private string playerName;

    void Start()
    {
        // Find the GameObject with the AttackPlayerState script
        GameObject playerObject = GameObject.Find(playerName); // Change "Player" to the name of your player GameObject
        if (playerObject != null)
        {
            // Get the AttackPlayerState component
            jumpPlayerState = playerObject.GetComponent<AttackPlayerState>();
            player = playerObject.GetComponent<Player>();
        }
    }

    // Call the return method when the attack animation finishes
    public void airReturn1()
    {
        Debug.Log("return1() method called");
        if (jumpPlayerState != null)
        {
            jumpPlayerState.return1();
            Debug.Log("AirReturn1 called");
        }
    }
    public void airReturn2()
    {
        if (jumpPlayerState != null)
        {
            jumpPlayerState.return2();
        }
    }
    public void airReturn3()
    {
        if (jumpPlayerState != null)
        {
            jumpPlayerState.return3();
        }
    }
}
