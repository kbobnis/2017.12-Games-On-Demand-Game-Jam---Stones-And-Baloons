using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StonesAndBaloons {
	public class Board : MonoBehaviour, DeathListener {
		[SerializeField] private GameObject stonesGO;
		[SerializeField] private GameObject tilePrefab;
		[SerializeField] private Baloon baloonPrefab;

		private List<List<Tile>> tiles = new List<List<Tile>>();
		private List<Baloon> baloons = new List<Baloon>();
		private readonly List<BoardListener> listeners = new List<BoardListener>();

		private void CreateTiles() {
			int w = 10;
			int h = 10;
			foreach (List<Tile> col in tiles) {
				foreach (Tile tile in col) {
					Destroy(tile);
				}
			}
			tiles.Clear();
			foreach (Baloon baloon in baloons) {
				Destroy(baloon);
			}
			baloons.Clear();

			for (int x = 0; x < w; x++) {
				List<Tile> column = new List<Tile>();
				for (int y = 0; y < h; y++) {
					GameObject tileGO = Instantiate(tilePrefab, stonesGO.transform);
					tileGO.transform.localPosition = new Vector3(x, y); //stonesGO is positioned and scaled in that way that so that local position will match the appropriate position 
					tileGO.name = string.Format("Tile {0}, {1}", x, y);
					Tile tile = tileGO.GetComponent<Tile>();
					tile.EnableWhirlwind(y != 0); //on the top row we don't want to have any wirlwind
					column.Add(tile);
				}
				tiles.Add(column);
			}
		}

		private void CreateStones() {
			foreach (List<Tile> col in tiles) {
				for (int i = 0; i < col.Count - 1; i++) {
					//in the last tile we want to have baloons
					Tile tile = col[i];
					tile.AddStone(0.3f);
				}
			}
		}

		public Tile GetTile(int x, int y) {
			return tiles[x][y];
		}

		private void CreateBaloons() {
			int count = 0;
			foreach (List<Tile> tileColumn in tiles) {
				Tile lastTile = tileColumn.Last();
				Baloon baloonGO = Instantiate(baloonPrefab).GetComponent<Baloon>();
				count++;
				baloonGO.transform.position = lastTile.transform.position;
				baloonGO.GetComponentInChildren<Baloon>().RegisterDeathListener(this);
				baloons.Add(baloonGO);
			}
		}

		public void ExplodeAllTiles() {
			StartCoroutine(ExplodeAllTilesInner());
		}

		private IEnumerator ExplodeAllTilesInner() {
			bool first = true;
			while (AnyTilesNotExploding()) {
				ExplodeFirstTile(first);
				first = !first;
				yield return new WaitForSeconds(0.05f);
			}
			yield return new WaitForSeconds(2f);

			while (AnyBaloonLeft()) {
				yield return new WaitForSeconds(0.5f);
			}

			foreach (BoardListener listener in listeners) {
				listener.NothingLeftOnBoard();
			}
		}

		private bool AnyBaloonLeft() {
			foreach (Baloon baloon in baloons) {
				if (baloon != null) {
					return true;
				}
			}
			return false;
		}

		private bool AnyTilesNotExploding() {
			foreach (List<Tile> col in tiles) {
				foreach (Tile tile in col) {
					if (tile.HasStone() && !tile.IsExploding()) {
						return true;
					}
				}
			}
			return false;
		}

		private void ExplodeFirstTile(bool front) {
			if (front) {
				foreach (List<Tile> column in tiles) {
					foreach (Tile tile in column) {
						if (tile.HasStone()) {
							tile.Explode();
							return;
						}
					}
				}
			} else {
				for (int i = tiles.Count - 1; i >= 0; i--) {
					List<Tile> column = tiles[i];
					for (int j = column.Count - 1; j >= 0; j--) {
						Tile tile = column[j];
						if (tile.HasStone()) {
							tile.Explode();
							return;
						}
					}
				}
			}
		}

		public void RegisterListener(BoardListener listener) {
			if (listener == null) {
				throw new ArgumentNullException();
			}
			if (listeners.Contains(listener)) {
				throw new ArgumentException(string.Format("Listener {0} already added.", listener));
			}
			this.listeners.Add(listener);
		}

		public void CreateLevel() {
			CreateTiles();
			CreateStones();
			CreateBaloons();
		}

		public void Died(Baloon baloon) {
			//check if any baloon is still left
			foreach (Baloon baloon1 in baloons) {
				if (!baloon1.Died()) {
					return;
				}
			}

			ExplodeAllTiles();
		}
	}
}