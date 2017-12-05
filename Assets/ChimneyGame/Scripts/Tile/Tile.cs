using System;
using System.Linq;
using UnityEngine;

namespace StonesAndBaloons {
	public class Tile : MonoBehaviour {
		[SerializeField] private Stone stonePrefab;
		[SerializeField] private Light light;
		[SerializeField] private GameObject whirlwind;
		[SerializeField] private GameObject explosionGameObject;

		private bool whirlwindEnabled;
		private bool isApplyingForce;
		private Stone stone;

		void Awake() {
			light.enabled = false;
			whirlwind.SetActive(false);
			explosionGameObject.SetActive(false);
		}

		public void AddStone(float health) {
			if (stone != null) {
				throw new InvalidOperationException("There is a stone already");
			}
			this.stone = Instantiate(this.stonePrefab, transform);
			this.stone.transform.Rotate(UnityEngine.Random.Range(0, 180), 0, 0);
			float[] possibilities = stonePrefab.softStonePrefab.Select(t => t.health).ToArray();
			float chosen = possibilities[UnityEngine.Random.Range(0, possibilities.Length)];
			this.stone.Init(chosen);
		}

		void Update() {
			if (stone == null) {
				whirlwind.SetActive(isApplyingForce && whirlwindEnabled);
			}
			
		}

		public void ApplyForce(bool enable) {
			isApplyingForce = enable;
			if (stone != null) {
				stone.SetCrush(isApplyingForce);
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

		public void EnableWhirlwind(bool b) {
			whirlwindEnabled = b;
		}
	}
}