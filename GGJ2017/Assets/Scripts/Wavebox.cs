using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavebox : MonoBehaviour {
    
	int r, select = 0;
	float timer;
	public float speed, delay;


	public float amplitudeY = 5.0f;
	public float omegaY = 5.0f;
	float index;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Sinewave Movement

		delay -= Time.deltaTime;

		if (delay < 0){
			index += Time.deltaTime;
			float y = Mathf.Abs (amplitudeY*Mathf.Sin (omegaY*index));
			transform.localPosition= new Vector3(transform.localPosition.x,y,transform.localPosition.z);	
		}



	}
}
