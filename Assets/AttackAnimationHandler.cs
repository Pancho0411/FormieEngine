using UnityEngine;

public class AttackAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private WalkPlayerState walkPlayerState;
    [SerializeField] private Player player;

    void Start()
    {
        // Find the GameObject with the AttackPlayerState script
        GameObject playerObject = GameObject.Find("Spark Player"); // Change "Player" to the name of your player GameObject
        if (playerObject != null)
        {
            // Get the AttackPlayerState component
            walkPlayerState = playerObject.GetComponent<WalkPlayerState>();
            player = playerObject.GetComponent<Player>();
        }
    }

    // Call the return method when the attack animation finishes
    public void return1()
    {
        Debug.Log("return1() method called");
        if (walkPlayerState != null)
        {
            walkPlayerState.return1();
            Debug.Log("Return1 called");
        }
    }
    public void return2()
    {
        if (walkPlayerState != null)
        {
            walkPlayerState.return2();
        }
    }
    public void return3()
    {
        if (walkPlayerState != null)
        {
            walkPlayerState.return3();
        }
    }
    public void return4()
    {
        if (walkPlayerState != null)
        {
            walkPlayerState.return4();
        }
    }
    public void return5()
    {
        if (walkPlayerState != null)
        {
            walkPlayerState.return5();
        }
    }
}
