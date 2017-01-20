using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveboxInstantiator : MonoBehaviour {

	public GameObject prefab;
	Vector3 spawnPos;
	public int waveLength, waveDepth;
	// Use this for initialization
	void Start () {
		spawnPos = transform.position;
		Wave ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Wave (){
		var wave = new GameObject ();
		wave.transform.position = spawnPos;
		wave.name = "Wave";

		for (int i = 0; i <waveLength; i++){
			for(int k = 0; k<waveDepth; k++){
				var box = Instantiate (prefab, new Vector3 (spawnPos.x + i, spawnPos.y, spawnPos.z + k), Quaternion.identity);
				box.transform.parent = wave.transform;
				box.GetComponent<Wavebox> ().delay = (i * 0.5f) + (k * 0.5f);
			}
		}

	}
}
