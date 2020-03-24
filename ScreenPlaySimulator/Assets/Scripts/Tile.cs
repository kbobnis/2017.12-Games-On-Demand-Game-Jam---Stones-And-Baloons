using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
		public int index;
		public FloorPad floorPad;
		public bool activated;
		public Color chromaticInactiveColor;
		public Color chromaticActiveColor;
		public Color monochromaticInactiveColor;
		public Color monochromaticActiveColor;
		public Color inactiveColor;
		public Color activeColor;
		public Transform tileNumber;
		public TextMesh tileNumberText;

		public void Initialize (int index, FloorPad floorPad, Color inactiveColor)
		{
				this.index = index;
				this.floorPad = floorPad;

				chromaticInactiveColor = inactiveColor;
				chromaticActiveColor = GetActiveColor (chromaticInactiveColor);
				monochromaticInactiveColor = Color.grey;
				monochromaticActiveColor = Color.white;
				
				tileNumber = transform.Find ("TileNumber");
				tileNumberText = tileNumber.GetComponent<TextMesh> ();
				tileNumberText.text = (index).ToString ();
				name = (index).ToString ();
				
				
		}

		void Start ()
		{
				ResetColor ();
		}

		public void SetChromatic (bool chromatic)
		{
				if (chromatic) {
						activeColor = chromaticActiveColor;
						inactiveColor = chromaticInactiveColor;
				} else {
						activeColor = monochromaticActiveColor;
						inactiveColor = monochromaticInactiveColor;
				}
				ResetColor ();
		}

		Color GetActiveColor (Color color)
		{
				HSBColor hsbColor = HSBColor.FromColor (color);
				hsbColor.s += .3f;
				hsbColor.b += .5f;
				return hsbColor.ToColor ();
		}

		public void Press ()
		{
				if (floorPad.toggleButtons) {
						activated = !activated;
				} else {
						activated = true;
				}
				//Debug.Log ("Pressed:" + activated);
				SendOSC ();
				ResetColor ();
		}

		public void Release ()
		{
				if (!floorPad.toggleButtons) {
						activated = false;
						SendOSC ();
				}
				//Debug.Log ("Released:" + activated);
				ResetColor ();
		}

		void SendOSC ()
		{
				floorPad.SendOSCMessage (index, activated);
		}

		void ResetColor ()
		{
				if (activated) {
						SetColor (activeColor);
				} else {
						SetColor (inactiveColor);
				}
		}

		void SetColor (Color color)
		{
				GetComponent<Renderer>().material.color = color;
		}
}
