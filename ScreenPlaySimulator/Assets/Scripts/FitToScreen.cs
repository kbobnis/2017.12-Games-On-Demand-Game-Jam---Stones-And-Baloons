using UnityEngine;
using System.Collections;

public class FitToScreen : MonoBehaviour
{
		public float screenWidth;
		public float screenHeight;
		public float worldHeight = 10f;

		void Update ()
		{
				if (screenWidth == Screen.width && screenHeight == Screen.height) 
						return;

				if (Screen.width >= Screen.height) {
						GetComponent<Camera>().orthographicSize = worldHeight / 2f;
				} else {
						GetComponent<Camera>().orthographicSize = (worldHeight / 2f) / Screen.width * Screen.height;
				}

				screenWidth = Screen.width;
				screenHeight = Screen.height;
		}
}
