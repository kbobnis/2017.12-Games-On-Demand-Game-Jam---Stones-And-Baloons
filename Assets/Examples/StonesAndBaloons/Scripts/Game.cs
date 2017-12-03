using UnityEngine;

namespace StonesAndBaloons {
	public class Game : MonoBehaviour, InputListener, BoardListener {

		[SerializeField] private AInput input;
		[SerializeField] private Board board;
		[SerializeField] private Ceiling ceiling;

		private const int WIDTH = 10;
		private const int HEIGHT = 10;

		private void Awake() {
			StartLevel();
			input.RegisterListener(this);
			board.RegisterListener(this);
		}

		private void StartLevel() {
			board.CreateLevel();
		}
		
		public void PressedAt(Vector2 coords) {
			GamePos pos = ScreenPosToGamePos(coords);
			board.GetTile(pos.x, pos.y).ApplyForce(true);
		}

		public void ReleasedAt(Vector2 coords) {
			GamePos pos = ScreenPosToGamePos(coords);
			board.GetTile(pos.x, pos.y).ApplyForce(false);
		}
		
		private GamePos ScreenPosToGamePos(Vector2 coords) {
			int xPos = Mathf.RoundToInt(coords.x * WIDTH);
			int yPos = Mathf.RoundToInt(coords.y * HEIGHT);
			return new GamePos(xPos, yPos);
		}

		public void NothingLeftOnBoard() {
			StartLevel();
		}
	}
}

