using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public Tile NorthTile;
    public Tile WestTile;
    public Tile SouthTile;
    public Tile EastTile;
    public Unit Occupier;
	public GameObject TileHighlight;
    public float height;
    GameObject grid;
    Grid gridScript;


    // Use this for initialization
    void Start () {
        grid = GameObject.Find("Grid");
        gridScript = grid.GetComponent<Grid>();
        height = transform.position.y + 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	// External method used to toggle the highlight graphic.
	public void toggleTileHighlight(string theColor) {

        Material texture;
        if (theColor.Equals("Orange"))
        {
            texture = Resources.Load("Highlight-Orange", typeof(Material)) as Material;
        }
        else if (theColor.Equals("Teal"))
        {
            texture = Resources.Load("Highlight-Teal", typeof(Material)) as Material;
        } else
        {
            texture = Resources.Load("Highlight-Red", typeof(Material)) as Material;
        }
        TileHighlight.GetComponent<Renderer>().material = texture;
        TileHighlight.SetActive (!TileHighlight.activeSelf);
	}

    // Mouse event method that triggers when a mouse click (left mouse button/mouse 1) hits a Tile's collider
    void OnMouseDown() {
        // Toggle the highlight graphic on the tile
        // TileHighlight.SetActive (!TileHighlight.activeSelf);

        // Accessing the Grid.
        gridScript.changeTileSelected(this);
		

		// If a Unit is selected we're attempting to move, so attempt a move. 
		if (gridScript.isUnitSelected ()) {
            if(gridScript.moving)
			    gridScript.attemptMove (new Vector3 (transform.position.x, height, transform.position.z), this);
            if (gridScript.attacking)
                gridScript.attemptAttack(new Vector3(transform.position.x, 1, transform.position.z), this);
		}
	}


		
}
