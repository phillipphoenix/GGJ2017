using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticlePulse : MonoBehaviour {
    public ParticleSystem particles;

    void Start () {
        Destroy(transform.parent.gameObject, particles.main.duration);
    }
}
