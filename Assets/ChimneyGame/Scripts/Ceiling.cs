using System;
using System.Collections.Generic;
using UnityEngine;

namespace StonesAndBaloons {
	public class Ceiling : MonoBehaviour {
		
		void OnTriggerEnter(Collider other) {
			if (other.attachedRigidbody != null && other.transform.parent != null &&
			    other.transform.parent.GetComponent<Baloon>() != null) {
				other.transform.parent.GetComponent<Baloon>().TurnToSparkles();
			}
		}
	}
}