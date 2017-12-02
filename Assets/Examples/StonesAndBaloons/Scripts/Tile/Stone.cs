using UnityEngine;

namespace StonesAndBaloons {
	public class Stone : MonoBehaviour {

		private float startingHealth;
		private float health;
		
		public void Init(float health) {
			this.startingHealth = this.health = health;
		}
		
		public void Crush(float deltaTime) {
			if (health > 0) {
				this.health -= deltaTime;
				if (health <= 0) {
					health = 0;
				}
				float f = this.health / startingHealth;
				this.transform.localScale = new Vector3(f, f, f);
			}
		}
	}
}