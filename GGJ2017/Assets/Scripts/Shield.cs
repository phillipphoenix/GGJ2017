using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Shield : MonoBehaviour {
    public SteamVR_TrackedController controller;
    public float duration;
    public float cooldown;
    public string waveLayer = "Waves";
    [Tooltip("Duration of the haptic pulse in SECONDS.")]
    public float hapticDuration = 0.5f;
    [Range(0.0f,1.0f)]
    public float hapticStrength = 0.5f;

    MeshFilter filter;
    MeshRenderer rend;
    SteamVR_Controller.Device device;
    Coroutine activeHapticCoroutine;

    void Start() {
        filter = GetComponent<MeshFilter>();
        rend = GetComponent<MeshRenderer>();
        device = SteamVR_Controller.Input((int)controller.controllerIndex);
        //SetupForWireframeShader();
        if (controller) {
            controller.TriggerClicked += EnableShield;
            controller.TriggerUnclicked += DisableShield;
        }
    }

    void SetupForWireframeShader() {
        // Set each vertex of a triangle to R G and B respectively
        Mesh m = filter.sharedMesh;
        m.colors = new Color[m.vertexCount];
        for (int i = 0; i < m.triangles.Length; ++i) {
            int tri = m.triangles[i];
            if (i % 3 == 0) {
                m.colors[tri] = Color.red;
            } else if (i % 3 == 1) {
                m.colors[tri] = Color.green;
            } else {
                m.colors[tri] = Color.blue;
            }
        }
        rend.material = new Material(Shader.Find("Custom/Wireframe"));
    }

    void EnableShield(object sender, ClickedEventArgs e) {

    }

    void DisableShield(object sender, ClickedEventArgs e) {

    }

    private void OnDisable() {
        if (controller) {
            controller.TriggerClicked -= EnableShield;
            controller.TriggerUnclicked -= DisableShield;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == LayerMask.NameToLayer(waveLayer)) {
            if(activeHapticCoroutine == null) {
                activeHapticCoroutine = StartCoroutine(StartHapticVibrationCoroutine(hapticDuration, hapticStrength));
            }            
            Destroy(other.gameObject);
        }
    }

    IEnumerator StartHapticVibrationCoroutine(float duration, float strength) {

        for (float i = 0; i < duration; i += Time.deltaTime) {
            device.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }

        activeHapticCoroutine = null;
    }
}
