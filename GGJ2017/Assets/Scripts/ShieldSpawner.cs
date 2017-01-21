using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(SteamVR_TrackedController), typeof(VRTK_ControllerActions))]
public class ShieldSpawner : MonoBehaviour {
    public Shield shieldPrefab;    

    public float shootRumbleDuration = 0.01f;
    public float shootRumbleInterval = 0.001f;   

    SteamVR_TrackedController controller;
    Rumbler rumbler;
    Shield heldShield;

    // Use this for initialization
    void Start () {
        controller = GetComponent<SteamVR_TrackedController>();
        rumbler = new Rumbler(this, GetComponent<VRTK_ControllerActions>());
        // Callbacks for trigger button
        if (controller) {
            controller.TriggerClicked += TriggerDown;
            controller.TriggerUnclicked += TriggerUp;            
        } else {
            Debug.LogError(this.GetType().Name + " needs a reference to a " + typeof(SteamVR_TrackedController).Name);
        }
    }

    void Update() {

    }

    void TriggerDown(object sender, ClickedEventArgs e) {
        CreateShield();
    }

    [ContextMenu("Charge Shield")]
    void CreateShield() {
        // Create and make child so it follows controller
        heldShield = Instantiate(shieldPrefab);        
        heldShield.rumbler = rumbler;
        heldShield.spawner = this;
    }

    void TriggerUp(object sender, ClickedEventArgs e) {        
        ShootShield();        
    }

    [ContextMenu("Shoot Shield")]
    void ShootShield() {
        // Unparent, shoot, forget about it
        if(heldShield != null) {
            heldShield.transform.parent = null;
            heldShield.Shoot();
            heldShield = null;
        }
        // Haptic feedback
        if (rumbler != null) {
            rumbler.Rumble(1, shootRumbleDuration, shootRumbleInterval);
        }
    }

    void OnDisable() {
        if (controller) {
            controller.TriggerClicked -= TriggerDown;
            controller.TriggerUnclicked -= TriggerUp;
        }
    }
}
