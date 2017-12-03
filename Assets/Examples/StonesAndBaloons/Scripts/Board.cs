using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StonesAndBaloons {
	public class Board : MonoBehaviour {
		[SerializeField] private GameObject stonesGO;
		[SerializeField] private GameObject tilePrefab;
		[SerializeField] private Baloon baloonPrefab;

		private List<List<Tile>> tiles = new List<List<Tile>>();
		private List<Baloon> baloons = new List<Baloon>();

		public void CreateTiles(int w, int h) {
			for (int x = 0; x < w; x++) {
				List<Tile> column = new List<Tile>();
				for (int y = 0; y < h; y++) {
					GameObject tileGO = Instantiate(tilePrefab, stonesGO.transform);
					tileGO.transform.localPosition =
						new Vector3(x,
							y); //stonesGO is positioned and scaled in that way that so that local position will match the appropriate position 
					tileGO.name = string.Format("Tile {0}, {1}", x, y);
					column.Add(tileGO.GetComponent<Tile>());
				}
				tiles.Add(column);
			}
		}

		public void CreateStones() {
			foreach (List<Tile> col in tiles) {
				for (int i = 0; i < col.Count - 1; i++) {
					//in the last we want to have baloons
					Tile tile = col[i];
					tile.AddStone(0.3f);
				}
			}
		}

		public Tile GetTile(int x, int y) {
			return tiles[x][y];
		}

		public void CreateBaloons() {
			foreach (List<Tile> tileColumn in tiles) {
				Tile lastTile = tileColumn.Last();
				Baloon baloonGO = Instantiate(baloonPrefab).GetComponent<Baloon>();
				baloonGO.transform.position = lastTile.transform.position;
				//baloonGO.transform.localScale = Vector3.one * 0.5f;
				baloons.Add(baloonGO);
			}
		}
	}
}