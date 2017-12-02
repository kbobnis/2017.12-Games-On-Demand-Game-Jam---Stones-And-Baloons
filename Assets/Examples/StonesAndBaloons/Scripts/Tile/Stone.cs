using System;
using UnityEngine;

namespace StonesAndBaloons {
	public class Stone : MonoBehaviour {
		[SerializeField] private ToughnessAndStone[] softStonePrefab;
		[SerializeField] private GameObject explosion;

		private float startingHealth;
		private float health;

		public void Init(float health) {
			ToughnessAndStone lastMatching = null;
			foreach (ToughnessAndStone healthAndStone in softStonePrefab) {
				if (healthAndStone.health == health) {
					lastMatching = healthAndStone;
				}
			}
			if (lastMatching == null) {
				throw new Exception(string.Format("There is no store prefab for health range {0}", health));
			}
			this.startingHealth = this.health = lastMatching.health;
			GameObject stone = Instantiate(lastMatching.prefab, this.transform);
			stone.transform.localPosition = Vector3.zero;
		}

		public void Crush(float deltaTime) {
			if (health > 0) {
				this.health -= deltaTime;
				if (health <= 0) {
					health = 0;
					
					Destroy(gameObject);
				} else {
					float f = this.health / startingHealth;
				}
			}
			transform.Rotate(new Vector3(0, 0, 1),  2 / health);
		}
	}

	[Serializable]
	public class ToughnessAndStone {
		[SerializeField] public float health;
		[SerializeField] public GameObject prefab;
	}
}