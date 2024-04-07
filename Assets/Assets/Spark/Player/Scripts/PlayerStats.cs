using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Freedom Engine/Player Stats", order = 0)]
public class PlayerStats : ScriptableObject
{
    [Header("General")]
    public float minAngleToRotate;
    public float minSpeedToSlide;
    public float minAngleToSlide;
    public float minAngleToFall;
    public float pushBack;
    public float diePushUp;
    public float invincibleTime;
    public float controlLockTime;
    public float topSpeed;
    public float maxSpeed;

    [Header("Ground")]
    public float acceleration;
    public float deceleration;
    public float friction;
    public float slope;
    public float minSpeedToBrake;
    public float turnSpeed;

    [Header("Air")]
    public float airAcceleration;
    public float gravity;

    [Header("Jump")]
    public float minJumpHeight;
    public float maxJumpHeight;

    [Header("Dash")]
    public float dashSpeed;
    public float downDashSpeed;

    [Header("DownStomp")]
    public float downMultiplier;

    [Header("Attack")]
    public float cooldownTime;
    public float lastPressedTime;
    public float numOfPresses;
    public float maxPresses;
    public float maxAirPresses;
}