
using UnityEngine;

namespace StonesAndBaloons {
	public class GamePos {
		public readonly int x,y;

		public GamePos(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public static GamePos FromCoords(Vector2 coords) {
			return new GamePos((int)coords.x, (int)coords.y);
		}

		public override string ToString() {
			return string.Format("x {0}, y {0}", this.x, this.y);
		}
	}
}