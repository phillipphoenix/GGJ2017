using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour {

	public float sensitivity = 100;
	public float loudness = 0;
	public float threshold = 2;
	public bool on = false;

	AudioSource src;

	void Start(){
		src = GetComponent<AudioSource> ();
		//src.volume = 0f;
		src.clip = Microphone.Start ("Built-in Microphone", true, 10, 44100);
		src.loop = true;
		while (!(Microphone.GetPosition ("Built-in Microphone") > 1)) {
		}
		src.Play();
	}
	void Update(){
		loudness = GetAveragedVolume () * sensitivity;

		if (loudness>threshold){
			on = true;
		}
		else {
			on = false;
		}

	}
	public float GetAveragedVolume()
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
}
