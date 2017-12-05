using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;

namespace StonesAndBaloons {
	public class ScreenPlayStoneAndBaloonsAInput : AInput {

		void Start() {
			Application.runInBackground = true;
		}

		void Update() {
			FloorPadInput.GetEvents(gameObject);
		}

		void OnTilePressed(Vector2 coords) {
			foreach (InputListener listener in inputListeners) {
				listener.PressedAt(Normalize(coords));
			}
		}

		void OnTileReleased(Vector2 coords) {
			foreach (InputListener listener in inputListeners) {
				listener.ReleasedAt(Normalize(coords));
			}
		}
		
		private Vector2 Normalize(Vector2 coords) {
			return coords / 10; //the screenplay mat has 10x10 and we're normalizing it to 1x1
		}
	}
}