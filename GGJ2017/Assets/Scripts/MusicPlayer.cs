using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour {

	public AudioClip[] parts;
	public AudioSource[] sources;
	public AudioMixerGroup[] groups;
	public AudioClip introClip;
	public AudioSource introSource;
	public AudioMixerGroup introGroup;

	public SfxPlayer sfx;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < parts.Length; i++){
			var go = gameObject.AddComponent <AudioSource>() as AudioSource;
			sources = gameObject.GetComponents<AudioSource> ();
			sources [i].clip = parts [i];
			sources [i].outputAudioMixerGroup = groups [i];
			sources [i].loop = true;
		}

		introSource = gameObject.AddComponent <AudioSource>() as AudioSource;
		introSource.clip = introClip;
		introSource.outputAudioMixerGroup = introGroup;


	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
			StartInGameMusic ();

		if (Input.GetKeyDown (KeyCode.S))
			StartIntroMusic ();

		if (Input.GetKeyDown (KeyCode.D))
			sfx.Death ();
		}

	public float GetAveragedVolume(AudioSource src)
	{ 
		float[] data = new float[256];
		float a = 0;
		src.GetOutputData(data,0);
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		return a/256;
	}

	public void StartInGameMusic(){
		introSource.Stop ();
		for (int k = 0; k < sources.Length; k++){
			sources [k].PlayScheduled (AudioSettings.dspTime + 0.5);
		}
	}

	public void StartIntroMusic(){
		for (int k = 0; k < sources.Length; k++){
			sources [k].Stop();
		}

		introSource.Play ();

	}

	public void StopAllMusic(){
		for (int k = 0; k < sources.Length; k++){
			sources [k].Stop();
		}
		introSource.Stop ();
	}
}
