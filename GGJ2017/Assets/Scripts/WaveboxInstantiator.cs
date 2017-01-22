using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveboxInstantiator : MonoBehaviour {

    [SerializeField]
	private GameObject _prefab;
    [SerializeField]
	private int _waveLength, _waveDepth, _segments;
	// Use this for initialization
	void Start () {
		Wave ();
	}

	void Wave (){
        var wave = new GameObject("Wave");
	    wave.transform.position = transform.position;
       // wave.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -45-180, 0));
        wave.transform.parent = transform;


		for(int s = 1; s < _segments + 1; s++){
			for (int i = 0; i <_waveLength; i++){
				for(int k = 0; k<_waveDepth; k++){
					var box = Instantiate (_prefab);
        	       	box.transform.parent = wave.transform;
					box.transform.localPosition = new Vector3(i, transform.position.y, k + (s * _waveDepth));
					box.GetComponent<Wavebox> ().delay = (i * 0.5f) + (s*2); // + (k * 0.5f);
					}
				}
		}
	}
}

