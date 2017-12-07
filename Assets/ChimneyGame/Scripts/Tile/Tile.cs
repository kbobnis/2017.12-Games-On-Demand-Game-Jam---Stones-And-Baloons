using System;
using System.Linq;
using UnityEngine;

namespace StonesAndBaloons {
	public class Tile : MonoBehaviour {
		[SerializeField] private Stone stonePrefab;
		[SerializeField] private Light light;
		[SerializeField] private GameObject explosionGameObject;

		public bool isApplyingForce { get; private set; }
		private Stone stone;

		void Awake() {
			light.enabled = false;
			explosionGameObject.SetActive(false);
			foreach (Transform child in transform) {
				//there was a bug that shoot component was not working after new game. 
				//but making this disabled and enabled in editor did the job
				child.gameObject.SetActive(!child.gameObject.activeSelf);
				child.gameObject.SetActive(!child.gameObject.activeSelf);
			}
		}

		public void AddStone() {
			if (stone != null) {
				throw new InvalidOperationException("There is a stone already");
			}
			this.stone = Instantiate(this.stonePrefab, transform);
			this.stone.transform.Rotate(UnityEngine.Random.Range(0, 180), 0, 0);
			float[] possibilities = stonePrefab.softStonePrefab.Select(t => t.health).ToArray();
			float chosen = possibilities[UnityEngine.Random.Range(0, possibilities.Length)];
			this.stone.Init(chosen);
		}

		public void ApplyForce(bool enable) {
			isApplyingForce = enable;
			if (stone != null) {
				stone.SetCrush(isApplyingForce);
			} else if (GetComponentInChildren<ShootComponent>() != null) {
				GetComponentInChildren<ShootComponent>().isApplyingForce = enable;
			}
			light.enabled = enable;
		}

		public bool HasStone() {
			return stone != null;
		}

		public void Explode() {
			if (stone != null) {
				Destroy(stone.gameObject);
				stone = null;
			}
			explosionGameObject.SetActive(true);
			Destroy(explosionGameObject, 2f);
		}

		public bool IsExploding() {
			return explosionGameObject != null && explosionGameObject.activeSelf;
		}
	}
}