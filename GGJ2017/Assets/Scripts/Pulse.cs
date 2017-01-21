using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
public class Pulse : MonoBehaviour {
    public AnimationCurve scale;
    public float lifetime = 1.0f;
    public float alpha = 0.5f;
    public MeshRenderer rend;
    public Material mat;

    float lived;

    void Start () {
        lived = 0;
        transform.localScale = Vector3.one * scale.Evaluate(lived);
        // Choose color
        //rend.material = new Material(mat);
        //rend.material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), alpha);
        
    }
	
	void Update () {        
        // Grow during lifetime
        lived += Time.deltaTime;
        transform.localScale = Vector3.one * scale.Evaluate(lived / lifetime);
        // Self destroy after duration
        if(lived > lifetime) {
            Destroy(gameObject);
        }
    }
}
