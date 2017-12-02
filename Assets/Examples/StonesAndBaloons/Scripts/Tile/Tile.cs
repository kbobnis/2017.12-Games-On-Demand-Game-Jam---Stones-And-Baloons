using System;
using UnityEngine;

namespace StonesAndBaloons {
	public class Tile : MonoBehaviour {

		[SerializeField] private Stone stonePrefab;

		private Stone stone;
		
		public void AddStone(float health) {
			if (stone != null) {
				throw new InvalidOperationException("There is a stone already");
			}
			this.stone = Instantiate(this.stonePrefab, transform);
			float[] possibilities = {0.3f, 1f, 3f};
			float chosen = possibilities[UnityEngine.Random.Range(0, possibilities.Length)];
			this.stone.Init(chosen);
		}

		public void ApplyForce(float power) {
			if (stone != null) {
				stone.Crush(power);
			}
		}
	}
}