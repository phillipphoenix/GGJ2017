using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour {

	public AudioClip[] parts;
	public AudioSource[] sources;
	public AudioMixerGroup[] groups;
	public AudioClip introClip, highscoreClip;
	public AudioSource introSource, highscoreSource;
	public AudioMixerGroup musicGroup;

    public static MusicPlayer Instance { get; set; }


    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        for (int i = 0; i < parts.Length; i++) {
            var go = gameObject.AddComponent<AudioSource>() as AudioSource;
            sources = gameObject.GetComponents<AudioSource>();
            sources[i].clip = parts[i];
            sources[i].outputAudioMixerGroup = groups[i];
            sources[i].loop = true;
        }

        introSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        introSource.clip = introClip;
        introSource.outputAudioMixerGroup = musicGroup;

        highscoreSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        highscoreSource.clip = highscoreClip;
        highscoreSource.outputAudioMixerGroup = musicGroup;


    }

	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
			StartInGameMusic ();

		if (Input.GetKeyDown (KeyCode.S))
			StartIntroMusic ();

		if (Input.GetKeyDown (KeyCode.D))
            SfxPlayer.Instance.Death ();
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
		highscoreSource.Stop ();
	}

	public void StartIntroMusic(){
		for (int k = 0; k < sources.Length; k++){
			sources [k].Stop();
		}

		highscoreSource.Stop ();

		introSource.Play ();

	}

	public void StartHighscoreMusic(){
		for (int k = 0; k < sources.Length; k++){
			sources [k].Stop();
		}

		highscoreSource.Play ();

	}

	public void StopAllMusic(){
		for (int k = 0; k < sources.Length; k++){
			sources [k].Stop();
		}
		introSource.Stop ();
	}
}
