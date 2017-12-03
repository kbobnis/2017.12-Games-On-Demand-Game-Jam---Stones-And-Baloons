using System;
using System.Collections.Generic;
using UnityEngine;

namespace StonesAndBaloons {
	public class Ceiling : MonoBehaviour {
		private readonly List<CeilingListener> listeners = new List<CeilingListener>();

		public void RegisterListener(CeilingListener listener) {
			if (listener == null) {
				throw new ArgumentNullException();
			}
			if (listeners.Contains(listener)) {
				throw new ArgumentException(string.Format("Listener {0} already added.", listener));
			}
			this.listeners.Add(listener);
		}

		void OnTriggerEnter(Collider other) {
			if (other.attachedRigidbody != null && other.transform.parent != null && other.transform.parent.GetComponent<Baloon>() != null) {
				foreach (CeilingListener ceilingListener in listeners) {
					ceilingListener.CeilingReached(other.transform.parent.GetComponent<Baloon>());
				}
			}
		}
	}
}