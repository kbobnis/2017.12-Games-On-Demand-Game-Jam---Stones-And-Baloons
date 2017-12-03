using System.Collections.Generic;
using UnityEngine;

namespace StonesAndBaloons {
	public class Game : MonoBehaviour, InputListener {

		[SerializeField] private AInput input;
		[SerializeField] private Board board;
		
		private readonly List<List<bool>> pressedMap = new List<List<bool>>();
		private const int WIDTH = 10;
		private const int HEIGHT = 10;

		private void Awake() {
			StartLevel();
			input.RegisterListener(this);
		}

		private void StartLevel() {
			pressedMap.Clear();
			for (int x = 0; x < WIDTH; x++) {
				pressedMap.Add(new List<bool>());
				for (int y = 0; y < HEIGHT; y++) {
					pressedMap[x].Add(false);
				}
			}
			board.CreateTiles(WIDTH, HEIGHT);
			board.CreateStones();
			//board.CreateBaloons();
		}
		
		public void PressedAt(Vector2 coords) {
			GamePos pos = ScreenPosToGamePos(coords);
			pressedMap[pos.x][pos.y] = true;
			Tile tile = board.GetTile(pos.x, pos.y);
			tile.ApplyForce(true);
		}

		public void ReleasedAt(Vector2 coords) {
			GamePos pos = ScreenPosToGamePos(coords);
			pressedMap[pos.x][pos.y] = false;
			Tile tile = board.GetTile(pos.x, pos.y);
			tile.ApplyForce(false);
		}
		
		private GamePos ScreenPosToGamePos(Vector2 coords) {
			int xPos = Mathf.RoundToInt(coords.x * WIDTH);
			int yPos = Mathf.RoundToInt(coords.y * HEIGHT);
			return new GamePos(xPos, yPos);
		}
	}
}

