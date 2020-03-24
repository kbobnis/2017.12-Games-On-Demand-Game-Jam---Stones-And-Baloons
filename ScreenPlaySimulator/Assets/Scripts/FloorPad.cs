using UnityEngine;
using System.Collections;
using System.Net;
using UnityOSC;
using UnityEngine.UI;
using System.Collections.Generic;

public class FloorPad : MonoBehaviour
{

		// Third party OSC library definitions. The ScreenPlay installation sends floor state
		// change information using OSC messages on port 5000. This simulator should as well.
		public string OSCClientIPAddress = "127.0.0.1";
		private OSCClient _OSCClient;
		private const int OSC_PORT = 5000;
		public bool toggleButtons;
		public bool showColors;
		public bool showGrid;
		public bool showNumbers;
		public Toggle toggleButtonsToggle;
		public Toggle toggleGridToggle;
		public Toggle toggleColorToggle;
		public InputField InputText;
		public Text InputTextPlaceHolder;

		// Arbitrary sizes for the on screen virtual floor
		public float lineWidth = 2;
		public Tile[] tiles;
		public int rows = 10;
		public int columns = 10;
		private float tileSize = 1;
		public GameObject TilePrefab;
		public GameObject LinePrefab;
		public Transform grid;
		private Transform tileHolder;
		public List<Tile> tilesPressed;
		public List<Tile> tilesPreviouslyPressed;
		
		void Awake ()
		{

				if (Application.platform == RuntimePlatform.Android) {
				} else {
						OSCClientIPAddress = "127.0.0.1";
				}
				InputTextPlaceHolder.text = "IP Address";
				InputText.text = OSCClientIPAddress;

				// Create the OSCClient object to send messages
				_OSCClient = new OSCClient (IPAddress.Parse (OSCClientIPAddress), OSC_PORT);


				tileHolder = GameObject.Find ("FloorPad").transform;

				tiles = new Tile[columns * rows];	
				for (int i=0; i<tiles.Length; i++) {
						int row = i / rows;
						int column = i % rows;
						int tileIndex = i + 1;
			
						GameObject tileGameObject = (GameObject)Instantiate (TilePrefab);
						tileGameObject.transform.position = new Vector3 (rows / 2f - row - tileSize / 2f, columns / 2f - column - tileSize / 2f, 0);
						tileGameObject.transform.parent = tileHolder;
						Tile tile = tileGameObject.GetComponent<Tile> ();	
						tile.Initialize (tileIndex, this, GetTileColour (tileIndex));
						tiles [i] = tile;
				}

				// Instantiate Grid
				CreateGrid ();

				// Initial settings for 

				showColors = true;
				showGrid = false;
				showNumbers = false;
				if (Application.platform == RuntimePlatform.Android) {
						toggleButtons = false;
				} else {
						toggleButtons = true;
					
				}

				toggleButtonsToggle.isOn = toggleButtons;
				toggleColorToggle.isOn = showColors;
				toggleGridToggle.isOn = showGrid;

		}
	
		public void UpdateIPAddress ()
		{
				OSCClientIPAddress = InputText.text.Trim ();
				_OSCClient = new OSCClient (IPAddress.Parse (OSCClientIPAddress), OSC_PORT);
		}

		// Use this for initialization
		void Start ()
		{
				SetTileColorVisibility ();
				SetTileNumberVisibility ();
				SetGridVisibility ();
				SetButtonToggleability ();
		}

		void CreateGrid ()
		{
				for (int row=0; row<=rows; row++) {
						GameObject line = (GameObject)Instantiate (LinePrefab);
						line.transform.localScale = new Vector3 (columns * tileSize, lineWidth, 1);
						line.transform.position = new Vector3 (0, row - columns * tileSize / 2f, -0.5f);
						line.transform.parent = grid;
				}
		
				for (int column=0; column<=columns; column++) {
						GameObject line = (GameObject)Instantiate (LinePrefab);
						line.transform.localScale = new Vector3 (lineWidth, rows * tileSize, 1);
						line.transform.position = new Vector3 (column - rows * tileSize / 2f, 0, -0.5f);
						line.transform.parent = grid;
				}
		}
	
		void Update ()
		{
				tilesPreviouslyPressed = new List<Tile> (tilesPressed);
				tilesPressed = new List<Tile> ();

				if (Input.touchCount > 0) {
						foreach (Touch touch in Input.touches) {
								Tile tile = GetTile (touch.position);
								if (tile != null) {
										if (!tilesPressed.Contains (tile)) {
												tilesPressed.Add (tile);
										}
								}
						}
				} else {
						if (Input.GetMouseButton (0)) {
								Tile tile = GetTile (Input.mousePosition);
								if (tile != null) {
										if (!tilesPressed.Contains (tile)) {
												tilesPressed.Add (tile);
										} 

								}
						}
				}

				foreach (Tile tile in tilesPressed) {
						if (!tilesPreviouslyPressed.Contains (tile)) {
								tile.Press ();
						}
				}

				foreach (Tile tile in tilesPreviouslyPressed) {
						if (!tilesPressed.Contains (tile)) {
								tile.Release ();
						}
				}



		}

		Tile GetTile (Vector2 position)
		{
				Collider2D collider2D = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (position));
				if (collider2D == null) {
						return null;
				}
				return collider2D.gameObject.GetComponent<Tile> ();
		}

		public void SetTileNumberVisibility ()
		{
				foreach (Tile tile in tiles) {
						tile.tileNumber.gameObject.SetActive (showNumbers);
				}
		}

		public void SetTileColorVisibility ()
		{
				foreach (Tile tile in tiles) {
						tile.SetChromatic (toggleColorToggle.isOn);
				}
		}

		public void SetGridVisibility ()
		{
				grid.gameObject.SetActive (toggleGridToggle.isOn);
		}

		public void SetButtonToggleability ()
		{
				toggleButtons = toggleButtonsToggle.isOn;
				if (!toggleButtons) {		
						ReleaseAllTiles ();
				}
		}

		void ReleaseAllTiles ()
		{
				foreach (Tile tile in tiles) {
						if (tile.activated) {
								tile.Release ();
						}
				}
		}

		void OnApplicationQuit ()
		{
				_OSCClient.Close ();
		}

		public void SendOSCMessage (int index, bool state)
		{
				// The format of the OSC message that ScreenPlay uses
				string _OSCAddress = "/1/push" + (index);
				OSCMessage msg = new OSCMessage (_OSCAddress, state ? 1.0f : 0.0f);
				_OSCClient.Send (msg);
		}

		Color GetTileColour (int tileNum)
		{
				// These are the colours of the actual carpet tiles used in the ScreenPlay
				// installation. RGB values are normalized for use with Unity.
				Color active = new Color (127.0f / 255.0f, 127.0f / 255.0f, 127.0f / 255.0f, 1.0f);   // colour of tiles that are being stepped on
				Color briteGreen = new Color (141.0f / 255.0f, 140.0f / 255.0f, 82.0f / 255.0f, 1.0f);  // colour of the brite green carpet tiles      
				Color calypso = new Color (125.0f / 255.0f, 58.0f / 255.0f, 95.0f / 255.0f, 1.0f);      // colour of the calypso carpet tiles
				Color marina = new Color (89.0f / 255.0f, 111.0f / 255.0f, 146.0f / 255.0f, 1.0f);      // colour of the marina carpet tiles
				Color portabella = new Color (127.0f / 255.0f, 110.0f / 255.0f, 92.0f / 255.0f, 1.0f);  // colour of the portabella carpet tiles

				switch (tileNum) {
				case 1:
						return calypso;
				case 2:
						return marina;
				case 3:
						return briteGreen;
				case 4:
						return calypso;
				case 5:
						return briteGreen;
				case 6:
						return marina;
				case 7:
						return portabella;
				case 8:
						return briteGreen;
				case 9:
						return marina;
				case 10:
						return briteGreen;
				case 11:
						return marina;
				case 12:
						return portabella;
				case 13:
						return portabella;
				case 14:
						return marina;
				case 15:
						return calypso;
				case 16:
						return portabella;
				case 17:
						return calypso;
				case 18:
						return portabella;
				case 19:
						return briteGreen;
				case 20:
						return marina;
				case 21:
						return calypso;
				case 22:
						return briteGreen;
				case 23:
						return marina;
				case 24:
						return briteGreen;
				case 25:
						return portabella;
				case 26:
						return briteGreen;
				case 27:
						return marina;
				case 28:
						return calypso;
				case 29:
						return marina;
				case 30:
						return calypso;
				case 31:
						return portabella;
				case 32:
						return marina;
				case 33:
						return portabella;
				case 34:
						return calypso;
				case 35:
						return briteGreen;
				case 36:
						return calypso;
				case 37:
						return briteGreen;
				case 38:
						return marina;
				case 39:
						return portabella;
				case 40:
						return briteGreen;
				case 41:
						return briteGreen;
				case 42:
						return calypso;
				case 43:
						return briteGreen;
				case 44:
						return marina;
				case 45:
						return calypso;
				case 46:
						return marina;
				case 47:
						return calypso;
				case 48:
						return marina;
				case 49:
						return briteGreen;
				case 50:
						return calypso;
				case 51:
						return briteGreen;
				case 52:
						return marina;
				case 53:
						return calypso;
				case 54:
						return portabella;
				case 55:
						return marina;
				case 56:
						return portabella;
				case 57:
						return briteGreen;
				case 58:
						return calypso;
				case 59:
						return marina;
				case 60:
						return portabella;
				case 61:
						return calypso;
				case 62:
						return briteGreen;
				case 63:
						return portabella;
				case 64:
						return calypso;
				case 65:
						return calypso;
				case 66:
						return marina;
				case 67:
						return portabella;
				case 68:
						return marina;
				case 69:
						return portabella;
				case 70:
						return briteGreen;
				case 71:
						return portabella;
				case 72:
						return calypso;
				case 73:
						return marina;
				case 74:
						return briteGreen;
				case 75:
						return portabella;
				case 76:
						return briteGreen;
				case 77:
						return marina;
				case 78:
						return briteGreen;
				case 79:
						return calypso;
				case 80:
						return calypso;
				case 81:
						return briteGreen;
				case 82:
						return portabella;
				case 83:
						return marina;
				case 84:
						return calypso;
				case 85:
						return briteGreen;
				case 86:
						return calypso;
				case 87:
						return briteGreen;
				case 88:
						return calypso;
				case 89:
						return portabella;
				case 90:
						return marina;
				case 91:
						return marina;
				case 92:
						return calypso;
				case 93:
						return briteGreen;
				case 94:
						return portabella;
				case 95:
						return marina;
				case 96:
						return marina;
				case 97:
						return calypso;
				case 98:
						return portabella;
				case 99:
						return briteGreen;
				case 100:
						return calypso;
				default:
						return active;
				}
		}
}
