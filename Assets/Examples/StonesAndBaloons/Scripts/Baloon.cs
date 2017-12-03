using UnityEngine;

namespace StonesAndBaloons {
	public class Baloon : MonoBehaviour {

		private float randomOffset;

		void Awake() {
			randomOffset = Random.Range(-1, 1);
		}
		
		private void FixedUpdate() {
			Vector3 actualForce = GetComponent<ConstantForce>().force;
			actualForce.x = Mathf.Sign(Mathf.Sin(randomOffset + Time.time * 1.5f )) / 10f;
			GetComponent<ConstantForce>().force = actualForce;
		}
	}
	
}