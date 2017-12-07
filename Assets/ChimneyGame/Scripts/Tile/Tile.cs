using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace StonesAndBaloons {
	public class Tile : MonoBehaviour {
		[SerializeField] private Stone stonePrefab;
		[SerializeField] private GameObject explosionGameObject;
		[SerializeField] private ShootComponent shootComponent;

		public GamePos myPos { get; private set; }
		private Board board;

		public bool isApplyingForce { get; private set; }
		private Stone stone;

		void Awake() {
			explosionGameObject.SetActive(false);
			GetComponentInChildren<Light>().enabled = false;
		}

		public void AddStone() {
			if (stone != null) {
				throw new InvalidOperationException("There is a stone already");
			}
			this.stone = Instantiate(this.stonePrefab, transform);
			float[] possibilities = stonePrefab.toughnessAndStone.Select(t => t.health).ToArray();
			float chosen = possibilities[UnityEngine.Random.Range(0, possibilities.Length)];
			this.stone.Init(chosen);
			
		}

		public void ApplyForce(bool enable) {
			isApplyingForce = enable;
			if (stone != null) {
				stone.SetCrush(isApplyingForce);
			} else if (GetComponentInChildren<ShootComponent>() != null) {
				GetComponentInChildren<ShootComponent>().isApplyingForce = enable;
			}
			
			bool lastPos = GetComponentInChildren<Light>().enabled;
			if (enable && lastPos != enable) {
				GetComponentInChildren<Light>().color = UnityEngine.Random.ColorHSV(0.4f, 0.7f);
			}
			GetComponentInChildren<Light>().enabled = enable;
		}

		public bool HasStone() {
			return stone != null;
		}

		public void Explode() {
			if (stone != null) {
				Destroy(stone.gameObject);
				stone = null;
			}
			explosionGameObject.SetActive(true);
			Destroy(explosionGameObject, 2f);
		}

		public bool IsExploding() {
			return explosionGameObject != null && explosionGameObject.activeSelf;
		}

		public void Init(GamePos gamePos, Board board) {
			myPos = gamePos;
			this.board = board;
			Destroy(shootComponent.GetComponent<Collider>());
			SphereCollider c = shootComponent.gameObject.AddComponent<SphereCollider>();
			c.isTrigger = true;
			c.radius = 0.5f;
		}

		public List<GamePos> GetFreeSides() {
			List<GamePos> allSides = new List<GamePos>{new GamePos(-1, 0), new GamePos(0, -1), new GamePos(1, 0), new GamePos(0, 1)};

			List<GamePos> freeSides = new List<GamePos>();
			foreach (GamePos side in allSides) {
				Tile tile = board.GetTileAtPos(myPos.x + side.x, myPos.y + side.y);
				if (tile != null && tile.HasNothing()) {
					freeSides.Add(side);
				}
			}
			return freeSides;
		}

		private bool HasNothing() {
			return stone == null;
		}
	}
}