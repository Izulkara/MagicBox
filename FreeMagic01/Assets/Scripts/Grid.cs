using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	private Tile currentlySelected;
    public BattleManager theBattleManager;
    bool Paused = false;

	// Use this for initialization
	void Start ()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (Paused == false)
            {
                Paused = true;
                Application.LoadLevel(1);
            }
            else
            {
                Paused = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (Paused == false)
            {
                Paused = true;
                Application.LoadLevel(1);
            }
            else
            {
                Paused = false;
            }
        }
    }

    public void moveUnit(Vector3 theVector){
        theBattleManager.moveUnit(theVector);
    }

    // Getter method for returning whether the current tile is selected or not
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

    // Toggles the highlight graphic for nearby tiles (N, E, S and W Tiles)
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
}
