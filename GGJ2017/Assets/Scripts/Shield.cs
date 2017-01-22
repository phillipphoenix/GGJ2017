using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(Collider))]
public class Shield : MonoBehaviour {   
    public string waveLayer = "Waves";
    [Tooltip("Duration of the haptic pulse in SECONDS.")]
    public GameObject pulsePrefab;

    // Charging params
    public AnimationCurve chargeCurve;
    public float chargeDuration;
    public Vector3 chargedScale;

    public Vector3 shieldOffset = new Vector3(0, 0, 5);
    public ShieldSpawner spawner;

    
    public float lifetime = 2.0f;
    public float speed = 100.0f;

    public float hitRumbleDuration = 0.5f;
    public float hitRumbleInterval = 0.001f;

    public float chargeRumbleInterval = 0.001f;

    

    float lived;    

    Collider col;

    public Rumbler rumbler;

    bool charging;

    void Start() {
        col = GetComponent<Collider>();
        charging = true;
        lived = 0;
        transform.localScale = chargedScale * chargeCurve.Evaluate(lived);
        // Charging sound feedback
        //rumbler.StartRumble(chargeDuration - 0.3f, chargeRumbleInterval);
    }

    void Update() {
        lived += Time.deltaTime;
        if (charging) {
            // Follow shield spawner
            transform.position = spawner.transform.position + spawner.transform.TransformDirection(shieldOffset);
            transform.rotation = spawner.transform.rotation;
            // Grow to maximum size
            transform.localScale = chargedScale * chargeCurve.Evaluate(lived / chargeDuration);
        } else {            
            // Move forward            
            transform.position += speed * Time.deltaTime * transform.forward;
            // Self destroy after lifetime seconds
            if(lived > lifetime) {
                Destroy(gameObject);
            }
        }     
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(waveLayer)) {
            HandleWaveCollision(col.ClosestPointOnBounds(other.transform.position), other.gameObject);
        }
    }   

    void HandleWaveCollision(Vector3 collisionCenter, GameObject wave) {        
        Destroy(wave);
        // Haptic feedback only when charging
        if(charging && rumbler != null) {
            rumbler.Rumble(1, hitRumbleDuration, hitRumbleInterval);
        }
        // Visual feedback
        Pulse(collisionCenter);

        // Held shield collision sound feedback
        if(SfxPlayer.Instance) {
            SfxPlayer.Instance.ShieldCollision();
        }
    }

    [ContextMenu("Test Shoot Shield")]
    public void Shoot() {
        // Reset time lived
        lived = 0;
        // Stop charging
        charging = false;
        // Stop charging haptic feedback
        rumbler.StopRumble();
    }

    void Pulse(Vector3 center) {
        // Instantiate pulse as child
        if(pulsePrefab != null) {
            GameObject go = Instantiate(pulsePrefab);
            go.transform.position = center;
            go.transform.LookAt(transform);
        }        
    }

}
