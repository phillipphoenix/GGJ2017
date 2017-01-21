using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavebox : MonoBehaviour {

	public Material[] mats;
	MeshRenderer rend;
	int r, select = 0;
	float timer;
	public float speed, delay;


	public float amplitudeY = 5.0f;
	public float omegaY = 5.0f;
	float index;

	// Use this for initialization
	void Start () {
		rend = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Noise Animation
		timer -= 0.1f;

		if(timer < 0){
			timer = speed;

			if(mats.Length > 1){
				while(select == r){
					r = Random.Range (0, mats.Length);	
				}	
			}

			select = r;
			rend.material = mats [select];

		}

		//Sinewave Movement

		delay -= 0.1f;

		if (delay < 0){
			index += Time.deltaTime;
			float y = Mathf.Abs (amplitudeY*Mathf.Sin (omegaY*index));
			transform.localPosition= new Vector3(transform.localPosition.x,y,transform.localPosition.z);	
		}



	}
}
