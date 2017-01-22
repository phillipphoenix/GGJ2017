using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxPlayer : MonoBehaviour {

	public AudioClip[] shieldCollisionClips;
	public AudioClip death, shieldShoot, damageTaken;
	AudioSource[] sources;
	public AudioMixerGroup sfxGroup;
	public int buffer;
	int sourceSelect = 0;

    public static SfxPlayer Instance { get; set; }


    private void Awake() {
        if(Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }       
    }

	// Use this for initialization
	void Start () {
		for (int i = 0; i < shieldCollisionClips.Length + buffer; i++){
			var go = gameObject.AddComponent <AudioSource>() as AudioSource;
			sources = gameObject.GetComponents<AudioSource> ();
			sources [i].outputAudioMixerGroup = sfxGroup;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.Space)){
			ShieldCollision ();
		}
		
	}

	public void ShieldCollision(){
		int clip = Random.Range (0, shieldCollisionClips.Length);
		sources [sourceSelect].clip = shieldCollisionClips[clip];
		sources [sourceSelect].Play ();
		sourceSelect++;

		if (sourceSelect >= sources.Length - 1)
			sourceSelect = 0;
		/*
		for(int i = 0; i < sources.Length; i++){
			if(!sources[i].isPlaying){
				sources [i].clip = shieldCollisionClips [clip];
				sources [i].Play ();
				break;
			}
			else if (i == sources.Length){
				int r = Random.Range (0, sources.Length - 1);
				sources [r].clip = shieldCollisionClips [clip];
				sources [r].Play ();
				Debug.Log ("random");
			}
		}
		*/
	}

	public void Death(){
        MusicPlayer.Instance.StopAllMusic();
        sources [sourceSelect].clip = death;
		sources [sourceSelect].Play ();
	}

	public void ShieldShoot(){
		sources [sourceSelect].clip = shieldShoot;
		sources [sourceSelect].Play ();
		sourceSelect++;
	}

	public void DamageTaken(){
		sources [sourceSelect].clip = damageTaken;
		sources [sourceSelect].Play ();
		sourceSelect++;
	}
}
