using System;
using System.Collections.Generic;
using UnityEngine;

namespace StonesAndBaloons {
	/// <summary>
	/// This is mono behaviour abstract class instead of interfact
	/// because we want to be able to hook derivitives in the inspector
	/// </summary>
	public abstract class AInput : MonoBehaviour {
		protected readonly List<InputListener> inputListeners = new List<InputListener>();

		public void RegisterListener(InputListener listener) {
			if (listener == null) {
				throw new ArgumentNullException();
			}
			if (inputListeners.Contains(listener)) {
				throw new ArgumentException(string.Format("Listener {0} already added.", listener));
			}
			this.inputListeners.Add(listener);
		}
	}
}