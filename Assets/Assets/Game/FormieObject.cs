using UnityEngine;

public class FormieObject : MonoBehaviour
{
    public virtual void OnRespawn() { }

    public virtual void OnPlayerMotorContact(PlayerMotor motor) { }
}
