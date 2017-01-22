using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	Transform _t;
	public float Speed;
	float _delay;
	private bool _doRotation = false;
	// Use this for initialization
	void Start () {
		_t = transform;
		_delay = Random.Range (0f, 5f);
		Speed += Random.Range (-10f, 10f);
	}
	
	// Update is called once per frame
	void Update () {

		if(_doRotation){
		_delay -= 0.1f;

		if(_delay < 0)
		    _t.Rotate (Vector3.forward * Time.deltaTime * Speed);
		}


	}

	public void StartRotation(){
		_doRotation = true;
	}
}
