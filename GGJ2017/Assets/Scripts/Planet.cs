using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

	Transform t;
	public float speed, pumpSpeed, threshold;
	public string direction;
	public int audioMixerGroup;
	public Vector3 baseSize;
	public Vector3 changeSize;
    
	// Use this for initialization
	void Start () {
		t = transform;
	}
	
	// Update is called once per frame
	void Update () {

		if(direction == "right")
			t.Rotate (Vector3.right * Time.deltaTime * speed);
		if(direction == "left")
			t.Rotate (Vector3.left * Time.deltaTime * speed);
		if(direction == "up")
			t.Rotate (Vector3.up * Time.deltaTime * speed);
		if(direction == "down")
			t.Rotate (Vector3.down * Time.deltaTime * speed);

		if(MusicPlayer.Instance.GetAveragedVolume (MusicPlayer.Instance.sources [audioMixerGroup]) > threshold){
			t.localScale = changeSize;
		}
		else if (t.localScale.x > baseSize.x){
			t.localScale = new Vector3(t.localScale.x - pumpSpeed, t.localScale.y - pumpSpeed, t.localScale.z - pumpSpeed);
		}
	}
}
