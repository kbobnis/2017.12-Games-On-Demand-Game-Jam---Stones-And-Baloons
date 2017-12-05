﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaySingleSound : MonoBehaviour {

	private static Dictionary<AudioClip, OneSound> Sounds = new Dictionary<AudioClip, OneSound>();

	private float SoundStart = 0f;
	public OneSound MySound;

	public static AudioSource SpawnSound( AudioClip clip, float volume=1){
		if (clip != null){
			if (!Sounds.ContainsKey(clip)) {
				Sounds.Add(clip, new OneSound(clip, volume));
			}
			return Sounds[clip].PlayAnother();
		}
		return null;
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

public class OneSound {

	private AudioClip AudioClip;
	private float Volume;
	public int ActuallyPlaying;
	private int MaxSimult = 5;
	private float MinDelay = 0.1f;
	private float LastPlay;

	public OneSound(AudioClip ac, float volume) {
		AudioClip = ac;
		Volume = volume;
	}

	public AudioSource PlayAnother() {
		if (ActuallyPlaying < MaxSimult && Time.time - MinDelay > LastPlay) {
			LastPlay = Time.time;
			ActuallyPlaying++;
			GameObject go = new GameObject("sound clip: " + AudioClip.name);
			AudioSource audio = go.AddComponent<AudioSource>();
			audio.volume = Volume;// -(Volume / MaxSimult * ActuallyPlaying);
			audio.panStereo = 0;// pan == 0 ? Random.Range(-1, 1) : pan;
			audio.clip = AudioClip;
			audio.spatialBlend = -1f;
			audio.Play();
			PlaySingleSound playSound = go.AddComponent<PlaySingleSound>();
			playSound.MySound = this;
			return audio;
		}
		return null;
	}


}