using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	Transform t;
	public float speed;
	float delay;
	public bool doRotation = false;
	// Use this for initialization
	void Start () {
		t = transform;
		delay = Random.Range (0f, 5f);
		speed += Random.Range (-10f, 10f);
	}
	
	// Update is called once per frame
	void Update () {

		if(doRotation){
		delay -= 0.1f;

		if(delay < 0)
		t.Rotate (Vector3.down * Time.deltaTime * speed);
		}
	}

	public void StartRotation(){
		doRotation = true;
	}
}
