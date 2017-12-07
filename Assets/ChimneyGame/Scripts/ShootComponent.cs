using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace StonesAndBaloons {
	public class ShootComponent : MonoBehaviour {
		[SerializeField] private float force;
		[SerializeField] private float shootDelay;
		[SerializeField] private GameObject animation;

		private readonly Dictionary<Baloon, float> baloonsInside = new Dictionary<Baloon, float>();

		public bool isApplyingForce { set; private get; }

		private void Awake() {
			if (!GetComponent<Collider>().isTrigger) {
				throw new Exception("Shot component must be trigger.");
			}
		}

		void Update() {
			animation.SetActive(transform.parent.GetComponent<Tile>().isApplyingForce);

			if (isApplyingForce) {
				List<Baloon> baloons = baloonsInside.Keys.ToList();
				foreach (Baloon baloon in baloons) {
					float time = baloonsInside[baloon];
					if (time + shootDelay < Time.time) {
						Rigidbody r = baloon.GetComponentInChildren<Rigidbody>();
						List<GamePos> freeSides = transform.parent.GetComponent<Tile>().GetFreeSides();
						GamePos side = freeSides[UnityEngine.Random.Range(0, freeSides.Count)];
						Vector3 forceVector = new Vector3(side.x, side.y, 0) * force;
						r.AddForce(forceVector);
						baloonsInside[baloon] = Time.time;
					}
				}
			}
		}

		void OnTriggerEnter(Collider other) {
			Baloon baloon = other.transform.parent.GetComponent<Baloon>();
			if (baloon != null && !baloonsInside.ContainsKey(baloon)) {
				baloonsInside.Add(baloon, 0);
			}
		}

		private void OnTriggerExit(Collider other) {
			Baloon baloon = other.transform.parent.GetComponent<Baloon>();
			if (baloon != null && baloonsInside.ContainsKey(baloon)) {
				baloonsInside.Remove(baloon);
			}
		}
	}
}