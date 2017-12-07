using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StonesAndBaloons {
	public class Baloon : MonoBehaviour {
		[SerializeField] private GameObject sparklesEffect;
		[SerializeField] private Material[] colorMaterials;
		
		private readonly List<DeathListener> listeners = new List<DeathListener>(); 

		private float randomOffset;
		private bool died;

		void Awake() {
			sparklesEffect.gameObject.SetActive(false);
			randomOffset = Random.Range(-1f, 1f);
		}

		private void FixedUpdate() {
			float x = Mathf.Sign(Mathf.Sin(randomOffset + Time.time * 0.3f)) * 0.7f;
			GetComponentInChildren<Rigidbody>().AddForce(x, 0, 0);
		}

		public void TurnToSparkles() {
			GetComponentInChildren<MeshRenderer>().enabled = false;
			GetComponentInChildren<Rigidbody>().isKinematic = true;
			GetComponentInChildren<Collider>().enabled = false;
			sparklesEffect.gameObject.SetActive(true);
			this.died = true;
			PlaySingleSound.SpawnSound(SoundManager.Me.BaloonsFinished);
			foreach (DeathListener deathListener in listeners) {
				deathListener.Died(this);
			}
			Destroy(gameObject, 2.5f);
		}

		private void RegisterDeathListener(DeathListener listener) {
			if (listener == null) {
				throw new ArgumentNullException();
			}
			if (listeners.Contains(listener)) {
				throw new ArgumentException(string.Format("Listener {0} already added.", listener));
			}
			this.listeners.Add(listener);
		}
		
		void OnTriggerEnter(Collider other) {
			Debug.LogFormat("baloon touched {0}", other.gameObject.name);
		}

		public bool Died() {
			return died;
		}

		public void Init(DeathListener listener) {
			RegisterDeathListener(listener);

			Material m = colorMaterials[UnityEngine.Random.Range(0, colorMaterials.Length)];
			float r = UnityEngine.Random.Range(0, 1f);
			float g = UnityEngine.Random.Range(0, 1f);
			float b = UnityEngine.Random.Range(0, 1f);
			m.color = new Color(r, g, b);// UnityEngine.Random.ColorHSV(0.4f, 1f);
			GetComponentInChildren<MeshRenderer>().materials = new Material[] { m };
		}
	}
}