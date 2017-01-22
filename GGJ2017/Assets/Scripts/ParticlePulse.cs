using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePulse : MonoBehaviour {
    public ParticleSystem particles;
    public bool DestroyAfterPulse = true;

    void Start () {
        if(DestroyAfterPulse) {
            Destroy(gameObject, particles.main.duration);
        }        
    }

    public void DoPulse () {
        particles.Play();
    }
}
