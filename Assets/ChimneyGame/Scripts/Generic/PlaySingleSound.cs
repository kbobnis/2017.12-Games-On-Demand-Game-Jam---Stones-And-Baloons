using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaySingleSound : MonoBehaviour {

	private static Dictionary<AudioClip, OneSound> Sounds = new Dictionary<AudioClip, OneSound>();

	private float SoundStart = 0f;
	public OneSound MySound;

	public static AudioSource SpawnSound( AudioClip clip, SoundOptions options = null) {
		options = options ?? new SoundOptions();
		if (clip != null){
			if (!Sounds.ContainsKey(clip)) {
				Sounds.Add(clip, new OneSound(clip, options));
			}
			return Sounds[clip].PlayAnother();
		}
		return null;
	}
	
	public static AudioSource SpawnSound(AudioClip[] clips, SoundOptions options = null) {
		AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Length)];
		return SpawnSound(clip, options);
	}

	void Update() {
		if (SoundStart == 0f && MySound != null) {
			SoundStart = Time.realtimeSinceStartup;
		}
		if (GetComponent<AudioSource>() != null && Time.realtimeSinceStartup - SoundStart > GetComponent<AudioSource>().clip.length) {
			MySound.ActuallyPlaying--;
			GameObject.Destroy(gameObject);
		}
	}
}

public class SoundOptions {
	public readonly float Volume = 1;
	public int MaxSimultaneous = 3;
	public float MinDelay = 0.1f;
	public bool Repeat;
}

public class OneSound {

	private AudioClip AudioClip;
	public int ActuallyPlaying;
	private float LastPlay;
	private SoundOptions Options;

	public OneSound(AudioClip ac, SoundOptions options) {
		AudioClip = ac;
		Options = options;
	}

	public AudioSource PlayAnother() {
		if (ActuallyPlaying < Options.MaxSimultaneous && Time.time - Options.MinDelay > LastPlay) {
			LastPlay = Time.time;
			ActuallyPlaying++;
			GameObject go = new GameObject("sound clip: " + AudioClip.name);
			AudioSource audio = go.AddComponent<AudioSource>();
			audio.volume = Options.Volume;// -(Volume / MaxSimult * ActuallyPlaying);
			audio.panStereo = 0;// pan == 0 ? Random.Range(-1, 1) : pan;
			audio.clip = AudioClip;
			audio.spatialBlend = -1f;
			audio.loop = Options.Repeat;
			audio.Play();
			PlaySingleSound playSound = go.AddComponent<PlaySingleSound>();
			playSound.MySound = this;
			return audio;
		}
		return null;
	}


}