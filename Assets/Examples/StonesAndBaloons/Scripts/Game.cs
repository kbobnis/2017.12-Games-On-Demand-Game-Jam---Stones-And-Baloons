using UnityEngine;

namespace StonesAndBaloons {
	public class Game : MonoBehaviour, InputListener, CeilingListener {

		[SerializeField] private AInput input;
		[SerializeField] private Board board;
		[SerializeField] private Ceiling ceiling;

		private int baloonsLeft;
		
		private const int WIDTH = 10;
		private const int HEIGHT = 10;

		private void Awake() {
			StartLevel();
			input.RegisterListener(this);
			ceiling.RegisterListener(this);
		}

		private void StartLevel() {
			board.CreateTiles(WIDTH, HEIGHT);
			board.CreateStones();
			baloonsLeft = board.CreateBaloons();
		}
		
		public void PressedAt(Vector2 coords) {
			GamePos pos = ScreenPosToGamePos(coords);
			Tile tile = board.GetTile(pos.x, pos.y);
			tile.ApplyForce(true);
		}

		public void ReleasedAt(Vector2 coords) {
			GamePos pos = ScreenPosToGamePos(coords);
			Tile tile = board.GetTile(pos.x, pos.y);
			tile.ApplyForce(false);
		}
		
		private GamePos ScreenPosToGamePos(Vector2 coords) {
			int xPos = Mathf.RoundToInt(coords.x * WIDTH);
			int yPos = Mathf.RoundToInt(coords.y * HEIGHT);
			return new GamePos(xPos, yPos);
		}

		public void CeilingReached(Baloon baloon) {
			baloonsLeft--;
			baloon.TurnToSparkles();
			if (baloonsLeft <= 0) {
				board.ExplodeAllTiles();
			}
		}
	}
}

