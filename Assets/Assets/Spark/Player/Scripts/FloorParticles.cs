using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FloorParticles : MonoBehaviour
{
    public PlayerParticles particles;

    //the following is used to play the footstep particles in the running animations
    public void PlayRunParticles()
    {
        particles.runSmoke.Play();
    }
    public void StopRunParticles()
    {
        if (particles.runSmoke != null)
        {
            particles.runSmoke.Stop();
        }
    }
}
