using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public Tile NorthTile;
    public Tile WestTile;
    public Tile SouthTile;
    public Tile EastTile;
    public GameObject Occupier;
	public GameObject Highlight;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// External method used to toggle the highlight graphic without a mouse event
	public void overrideHighlight() {
		Highlight.SetActive (!Highlight.activeSelf);
	}

    // Mouse event method that triggers when a mouse click (left mouse button/mouse 1) hits a collider
	void OnMouseDown() {
		// Toggle the highlight graphic on the tile
		Highlight.SetActive (!Highlight.activeSelf);

		// Tell the grid that this tile has been selected
		// If the grid has a tile selected tell the grid to attempt to move to this position
		GameObject grid = GameObject.Find ("Grid");
		Grid gridScript = grid.GetComponent<Grid> ();
		gridScript.changeSelected(this);

	}


		
}
