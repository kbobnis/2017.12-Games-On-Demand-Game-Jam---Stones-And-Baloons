using UnityEngine;

namespace StonesAndBaloons {
	public class SoundManager : MonoBehaviour {
		[SerializeField] public AudioClip Bang;
		[SerializeField] public AudioClip StoneCrushing;
		[SerializeField] public AudioClip Wind;
		[SerializeField] public AudioClip[] BaloonsFinished;
		[SerializeField] public AudioClip[] LevelSuccess;
		
		public static SoundManager Me;

		void Awake() {
			Me = this;
		}
	}
}