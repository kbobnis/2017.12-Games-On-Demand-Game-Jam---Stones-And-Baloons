using System.Collections.Generic;
using UnityEngine;

namespace StonesAndBaloons {
	public class Board : MonoBehaviour {

		[SerializeField] private GameObject stonesGO;
		[SerializeField] private GameObject tilePrefab;

		private List<List<Tile>> tiles = new List<List<Tile>>();
		
		public void CreateStones(int w, int h) {
			for (int x = 0; x < w; x++) {
				List<Tile> column = new List<Tile>();
				for (int y = 0; y < h; y++) {
					GameObject tileGO = Instantiate(tilePrefab, stonesGO.transform);
					tileGO.transform.localPosition = new Vector3(x, y); //stonesGO is positioned and scaled in that way that so that local position will match the appropriate position 
					tileGO.name = string.Format("Tile {0}, {1}", x, y);
					tileGO.GetComponent<Tile>().AddStone(1);
					column.Add(tileGO.GetComponent<Tile>());
				}
				tiles.Add(column);
			}
		}

		public Tile GetTile(int x, int y) {
			return tiles[x][y];
		}
	}
}