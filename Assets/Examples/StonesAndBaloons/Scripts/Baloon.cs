using UnityEngine;

namespace StonesAndBaloons {
	public class Baloon : MonoBehaviour {

		[SerializeField] private GameObject sparklesEffect;

		private float randomOffset;

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
			Destroy(gameObject, 2.5f);
		}
	}
}