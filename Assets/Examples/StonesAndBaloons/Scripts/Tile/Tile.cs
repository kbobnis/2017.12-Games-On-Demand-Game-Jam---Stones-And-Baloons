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
			this.stone.Init(health);
		}

		public void ApplyForce(float power) {
			if (stone != null) {
				stone.Crush(power);
			}
		}
	}
}