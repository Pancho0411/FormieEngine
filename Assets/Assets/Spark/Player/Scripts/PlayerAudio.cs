using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerAudio", menuName = "Formie Engine/Player Audio", order = 1)]
public class PlayerAudio : ScriptableObject
{
    public AudioClip jump;
    public AudioClip brake;
    public AudioClip dead;
    public AudioClip hurt;
    public AudioClip dash;
    public AudioClip downDashLand;
    public AudioClip attack;
}