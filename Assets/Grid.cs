using UnityEngine;
using System.Collections;


public class Grid : MonoBehaviour
{
    private Tile[,] tiles2D;
	private Tile currentlySelected;

	// Use this for initialization
	void Start ()
    {
        tiles2D = new Tile[10, 10];
        Tile[] tiles = gameObject.GetComponentsInChildren<Tile>();

        foreach (Tile t in tiles)
        {
            tiles2D[(int)t.transform.localPosition.x,(int)t.transform.localPosition.z] = t;
            
        }

        
	}

	public void attemptMove() {

	}

	public bool tileSelected() {
		return currentlySelected != null;
	}

	// Changes the currently selected tile on the grid 
	public void changeSelected(Tile newSelected) {
		// if we have a tile selected, unhighlight the tiles highlighted for movement
		// and unhighlight the currently selected tile
		if (currentlySelected != null) {
			currentlySelected.overrideHighlight ();
			toggleHighlightForMovement ();
		}

		// set the new tile to be the currently selected
		// toggle the highlighting for its movement 
		currentlySelected = newSelected;
		toggleHighlightForMovement ();
	}

	private void toggleHighlightForMovement() {
		if (currentlySelected.NorthTile != null) {
			currentlySelected.NorthTile.overrideHighlight ();
		} 

		if (currentlySelected.SouthTile != null) {
			currentlySelected.SouthTile.overrideHighlight ();
		}

		if (currentlySelected.EastTile != null) {
			currentlySelected.EastTile.overrideHighlight ();
		}

		if (currentlySelected.WestTile != null) {
			currentlySelected.WestTile.overrideHighlight ();
		}
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }
}
