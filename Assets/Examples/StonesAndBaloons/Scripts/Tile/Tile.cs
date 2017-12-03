using System;
using System.Linq;
using UnityEngine;

namespace StonesAndBaloons {
	public class Tile : MonoBehaviour {
		[SerializeField] private Stone stonePrefab;
		[SerializeField] private Light light;
		[SerializeField] private GameObject whirlwind;

		private bool isApplyingForce;
		private Stone stone;

		void Awake() {
			light.enabled = false;
			whirlwind.SetActive(false);
		}

		public void AddStone(float health) {
			if (stone != null) {
				throw new InvalidOperationException("There is a stone already");
			}
			this.stone = Instantiate(this.stonePrefab, transform);
			float[] possibilities = stonePrefab.softStonePrefab.Select(t => t.health).ToArray();
			float chosen = possibilities[UnityEngine.Random.Range(0, possibilities.Length)];
			this.stone.Init(chosen);
		}

		void Update() {
			light.enabled = isApplyingForce && stone != null;
			whirlwind.SetActive(isApplyingForce);
			if (stone != null) {
				stone.SetCrush(isApplyingForce);
			}
		}

		public void ApplyForce(bool enable) {
			isApplyingForce = enable;
		}
	}
}