﻿using System;
using UnityEngine;

namespace StonesAndBaloons {
	public class Stone : MonoBehaviour {
		[SerializeField] public ToughnessAndStone[] softStonePrefab;
		[SerializeField] private GameObject explosion;

		private AudioSource crushing;
		private bool isCrushed;
		private float startingHealth;
		private float health;

		public void Init(float health) {
			ToughnessAndStone lastMatching = null;
			foreach (ToughnessAndStone healthAndStone in softStonePrefab) {
				if (healthAndStone.health == health) {
					lastMatching = healthAndStone;
				}
			}
			if (lastMatching == null) {
				throw new Exception(string.Format("There is no store prefab for health range {0}", health));
			}
			this.startingHealth = this.health = lastMatching.health;
			GameObject stone = Instantiate(lastMatching.prefab, this.transform);
			stone.transform.localPosition = Vector3.zero;
		}

		void Update() {

			if (isCrushed) {
				if (health > 0) {
					this.health -= Time.deltaTime;
					if (health <= 0) {
						health = 0;
						if (this.crushing != null) {
							this.crushing.Stop();
						}
						PlaySingleSound.SpawnSound(SoundManager.Me.Bang);
						Destroy(gameObject);
					}
				}
				float angle =  Mathf.Sin(Time.time / Mathf.Pow(health, 0.5f)) * 10 ;
				transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
			} else {
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 1);
			}

		}

		public void SetCrush(bool isCrushed) {
			this.isCrushed = isCrushed;
			if (health > 0 && isCrushed) {
				this.crushing = PlaySingleSound.SpawnSound(SoundManager.Me.StoneCrushing);
			} else {
				if (this.crushing != null) {
					this.crushing.Stop();
					this.crushing = null;
				}
			}
		}
	}

	[Serializable]
	public class ToughnessAndStone {
		[SerializeField] public float health;
		[SerializeField] public GameObject prefab;
	}
}