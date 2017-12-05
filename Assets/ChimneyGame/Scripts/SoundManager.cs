using UnityEngine;

namespace StonesAndBaloons {
	public class SoundManager : MonoBehaviour {
		[SerializeField] public AudioClip Bang;
		[SerializeField] public AudioClip StoneCrushing;

		public static SoundManager Me;
		

		void Awake() {
			Me = this;
		}
	}
}