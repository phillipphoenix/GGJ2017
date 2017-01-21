using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticlePulse : MonoBehaviour {
    public ParticleSystem particles;

    void Start () {
        Destroy(gameObject, particles.main.duration);
    }
}
