using System;
using System.Collections.Generic;
using UnityEngine;

namespace StonesAndBaloons {
	public class WindComponent : MonoBehaviour {
		// To use:
		// 1. Create an empty game object
		// Add a Collider2D to the empty object. I used a standard Box Collider 2D, as a trigger, to create an isolated square that applies "wind"
		// 3. Add this WindComponent to the empty object, then adjust the Force (Vector2). You can adjust this in the editor or in the script, as it is a public property
		// Note: Only works on game objects that have the Rigid Body 2D and Collider 2D components

		// Internal list that tracks objects that enter this object's "zone"
		private List<Collider> objects = new List<Collider>();

		private void Awake() {
			if (!GetComponent<BoxCollider>().isTrigger) {
				throw new Exception("Wind collider must be trigger.");
			}
		}

		private void OnEnable() {
			objects.Clear();
		}

		// This function is called every fixed framerate frame
		void FixedUpdate() {
			// For every object being tracked
			for (int i = 0; i < objects.Count; i++) {
				// Get the rigid body for the object.
				Rigidbody body = objects[i].attachedRigidbody;

				// Apply the force
				Vector3 distance = transform.position - objects[i].transform.position;
				distance.z = 0;
				float force = 1 / (distance.magnitude * distance.magnitude) / 2f;
				if (force > 2) {
					force = 2;
				}
				Vector3 normalized = (distance).normalized;
				if (normalized.y < 0) {
					force *= 3; //to make the easy to drag down
				}
				Debug.LogFormat("Distance {0}. Adding force : {1} to {2}", distance, normalized, objects[i].name);
				body.AddForce(normalized * force);
			}
		}

		void OnTriggerEnter(Collider other) {
			if (other.attachedRigidbody != null && other.transform.parent.GetComponent<Baloon>() != null) {
				objects.Add(other);
			}
		}

		void OnTriggerExit(Collider other) {
			if (other.attachedRigidbody != null && other.transform.parent.GetComponent<Baloon>() != null) {
				objects.Remove(other);
			}
		}
	}
}