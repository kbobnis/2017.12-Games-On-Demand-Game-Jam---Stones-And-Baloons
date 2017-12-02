
using UnityEngine;

namespace StonesAndBaloons {
	public interface InputListener {
		/// <param name="coords">0,0 is top left. 1,1 is bottom right</param>
		void PressedAt(Vector2 coords);
		/// <param name="coords">0,0 is top left. 1,1 is bottom right</param>
		void ReleasedAt(Vector2 coords);
	}
}