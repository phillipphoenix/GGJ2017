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

    MeshFilter filter;
    MeshRenderer rend;

    void Start() {
        filter = GetComponent<MeshFilter>();
        rend = GetComponent<MeshRenderer>();
        SetupForWireframeShader();
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
            Destroy(other.gameObject);
        }
    }
}
