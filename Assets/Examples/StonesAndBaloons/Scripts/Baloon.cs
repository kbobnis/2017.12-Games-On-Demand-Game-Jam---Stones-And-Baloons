using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StonesAndBaloons {
	public class Baloon : MonoBehaviour {
		[SerializeField] private GameObject sparklesEffect;
		
		private readonly List<DeathListener> listeners = new List<DeathListener>(); 

		private float randomOffset;
		private bool died;

		void Awake() {
			sparklesEffect.gameObject.SetActive(false);
			randomOffset = Random.Range(-1, 1);
		}

		private void FixedUpdate() {
			Vector3 actualForce = GetComponent<ConstantForce>().force;
			actualForce.x = Mathf.Sign(Mathf.Sin(randomOffset + Time.time * 1.5f)) / 10f;
			GetComponent<ConstantForce>().force = actualForce;
		}

		public void TurnToSparkles() {
			GetComponentInChildren<MeshRenderer>().enabled = false;
			GetComponent<Rigidbody>().isKinematic = true;
			GetComponentInChildren<Collider>().enabled = false;
			sparklesEffect.gameObject.SetActive(true);
			this.died = true;
			foreach (DeathListener deathListener in listeners) {
				deathListener.Died(this);
			}
			Destroy(gameObject, 2.5f);
		}

		public void RegisterDeathListener(DeathListener listener) {
			if (listener == null) {
				throw new ArgumentNullException();
			}
			if (listeners.Contains(listener)) {
				throw new ArgumentException(string.Format("Listener {0} already added.", listener));
			}
			this.listeners.Add(listener);
		}

		public bool Died() {
			return died;
		}
	}
}