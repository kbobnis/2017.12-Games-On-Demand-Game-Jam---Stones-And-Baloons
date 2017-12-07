using UnityEngine;

namespace StonesAndBaloons {
	public class Ceiling : MonoBehaviour {
		
		void OnTriggerEnter(Collider other) {
			if (other.attachedRigidbody != null && other.transform.parent != null &&
			    other.transform.parent.GetComponentInChildren<Baloon>() != null) {
				other.transform.parent.GetComponentInChildren<Baloon>().TurnToSparkles();
			}
		}
	}
}