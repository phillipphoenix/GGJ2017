using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class Shield : MonoBehaviour {
    public SteamVR_TrackedController controller;
    
    
    public string waveLayer = "Waves";
    [Tooltip("Duration of the haptic pulse in SECONDS.")]
    public float hapticDuration = 0.5f;
    [Range(0.0f, 1.0f)]
    public float hapticStrength = 0.5f;

    public GameObject pulsePrefab;

    public AnimationCurve chargeCurve;
    public float chargeDuration;
    public Vector3 chargedScale;

    public float shootDuration = 2.0f;
    public float shootSpeed = 1.0f;
    public Vector3 shieldOffset = new Vector3(0, 0, 5);

    public float shootRumbleDuration = 0.5f;
    public float shootRumbleInterval = 0.001f;

    public float hitRumbleDuration = 0.5f;
    public float hitRumbleInterval = 0.001f;

    Collider col;
    MeshRenderer rend;

    bool flying = false;
    Transform pulse;

    Coroutine activePulseCoroutine;
    Coroutine activeChargeCoroutine;

    Rumbler rumble;

    void Start() {
        // Shield starts disabled.
        col = GetComponent<Collider>();
        rend = GetComponent<MeshRenderer>();
        DisableShield();
        // Callbacks for trigger button
        if (controller) {
            controller.TriggerClicked += TriggerDown;
            controller.TriggerUnclicked += TriggerUp;

            rumble = new Rumbler(this, controller.GetComponent<VRTK_ControllerActions>());
        } else {
            Debug.LogError(this.GetType().Name + " needs a reference to a " + typeof(SteamVR_TrackedController).Name);
        }
    }

    void Update() {
        // Follow controller
        if(controller != null && !flying) {
            transform.position = controller.transform.position + controller.transform.TransformDirection(shieldOffset);
            transform.rotation = controller.transform.rotation;
        }
    }

    void OnDisable() {
        if (controller) {
            controller.TriggerClicked -= TriggerDown;
            controller.TriggerUnclicked -= TriggerUp;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(waveLayer)) {
            HandleWaveCollision(col.ClosestPointOnBounds(other.transform.position), other.gameObject);
        }
    }

    void TriggerDown(object sender, ClickedEventArgs e) {
        ChargeShield();
    }

    void TriggerUp(object sender, ClickedEventArgs e) {
        // Stop charging rumble
        rumble.StopRumbleBuildUp();
        Debug.Log("Stop rumble");
        ShootShield();
    }    

    void HandleWaveCollision(Vector3 collisionCenter, GameObject wave) {        
        Destroy(wave);
        // Haptic feedback
        if(!flying) {
            rumble.Rumble(1, hitRumbleDuration, hitRumbleInterval);
        }        
        // Visual feedback
        Pulse(collisionCenter);
    }

    [ContextMenu("Test Charge Shield")]
    void ChargeShield() {
        if (activeChargeCoroutine == null && !flying) {
            activeChargeCoroutine = StartCoroutine(ChargeShieldCo());
        }        
    }

    IEnumerator ChargeShieldCo() {
        // Enable
        EnableShield();
        // Haptic feedback
        rumble.StartRumbleBuildUp(chargeDuration);
        // Scale
        for (float t = 0; t < chargeDuration; t += Time.deltaTime) {
            transform.localScale = chargedScale * chargeCurve.Evaluate(t / chargeDuration);
            yield return null;
        }
        activeChargeCoroutine = null;
    }

    [ContextMenu("Test Shoot Shield")]
    void ShootShield() {
        if (!flying) {
            // Stop charging and shoot shield
            if(activeChargeCoroutine != null) {
                StopCoroutine(activeChargeCoroutine);
            }            
            activeChargeCoroutine = null;
            StartCoroutine(ShootShieldCo());
        }
    }

    IEnumerator ShootShieldCo() {
        // Haptic feedback
        rumble.Rumble(1, shootRumbleDuration, shootRumbleInterval);
        // Scale
        flying = true;
        for (float t = 0; t < shootDuration; t += Time.deltaTime) {
            transform.position += shootSpeed * Time.deltaTime * transform.forward;
            yield return null;
        }
        flying = false;
        DisableShield();
    }

    void Pulse(Vector3 center) {
        // Instantiate pulse as child
        if(pulsePrefab != null) {
            GameObject go = Instantiate(pulsePrefab);
            go.transform.parent = transform;
            go.transform.position = center;
        }        
    }

    

    void DisableShield() {
        rend.enabled = false;
        col.enabled = false;
        transform.localScale = Vector3.one * chargeCurve.Evaluate(0);
    }

    void EnableShield() {
        rend.enabled = true;
        col.enabled = true;
    }
}
