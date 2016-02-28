using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public Tile NorthTile;
    public Tile WestTile;
    public Tile SouthTile;
    public Tile EastTile;
    public Unit Occupier;
	public GameObject TileHighlight;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// External method used to toggle the highlight graphic.
	public void toggleTileHighlight() {
		TileHighlight.SetActive (!TileHighlight.activeSelf);
	}
		
    // Mouse event method that triggers when a mouse click (left mouse button/mouse 1) hits a Tile's collider
	void OnMouseDown() {
		// Toggle the highlight graphic on the tile
		// TileHighlight.SetActive (!TileHighlight.activeSelf);

		// Accessing the Grid.
		GameObject grid = GameObject.Find ("Grid");
		Grid gridScript = grid.GetComponent<Grid> ();

		// If a Unit is selected we're attempting to move, so attempt a move. 
		if (gridScript.isUnitSelected ()) {
			gridScript.attemptMove (new Vector3 (transform.position.x, 1, transform.position.z), this);
		}
	}


		
}
