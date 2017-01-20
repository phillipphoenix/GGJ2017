using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavebox : MonoBehaviour {

	public Material[] mats;
	MeshRenderer renderer;
	int r, select = 0;
	float timer;
	public float speed, delay;


	public float amplitudeY = 5.0f;
	public float omegaY = 5.0f;
	float index;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Noise Animation
		timer -= 0.1f;

		if(timer < 0){
			timer = speed;

			while(select == r){
				r = Random.Range (0, mats.Length - 1);	
			}
			select = r;
			renderer.material = mats [select];

		}

		//Sinewave Movement

		delay -= 0.1f;

		if (delay < 0){
			index += Time.deltaTime;
			float y = Mathf.Abs (amplitudeY*Mathf.Sin (omegaY*index));
			transform.localPosition= new Vector3(transform.localPosition.x,y,0);	
		}



	}
}
