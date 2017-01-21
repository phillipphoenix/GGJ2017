using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerContainer : MonoBehaviour {

	public MusicPlayer music;
	public int audioMixerGroup;
	float shake;
	public float shakeFactor;
	public Vector3[] originalPos;
	public Transform[] sides;

	void Start () {

		sides = GetComponentsInChildren<Transform>().Where(t => t.gameObject.GetInstanceID() != gameObject.GetInstanceID()).ToArray();
		originalPos = new Vector3[sides.Length];
		for (int i = 0; i < sides.Length; i++){
			originalPos[i] = sides [i].position;
		}
	}

	void Update () {
	
		shake = music.GetAveragedVolume (music.sources [audioMixerGroup]) * shakeFactor;

		for(int i = 0; i < sides.Length; i++){
			sides [i].position = new Vector3 (originalPos[i].x + shake, originalPos[i].y + shake, originalPos[i].z + shake);
		}

	}
}
