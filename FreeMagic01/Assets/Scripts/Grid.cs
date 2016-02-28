using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	private Tile tileSelected;
	private Unit unitSelected;
    public BattleManager theBattleManager;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {

    }

	// Attempts to move a Unit to the given desiredTileLocation.
	// 
	// Determines whether the Unit can be moved based on the distance of the Unit's Tile position
	// relative to the desiredTileLocation's position. If the distance is less than or equal to the 
	// Unit's move range then the move is executed. 
	public void attemptMove(Vector3 theVector, Tile desiredTileLocation) {
		float distance = Vector3.Distance (unitSelected.myVector, theVector);
		print ("Distance: " + distance);
		// if the distance of the Tile we clicked on is less than or equal to our moveRange, then move
		if (distance <= unitSelected.unitMoveRange && desiredTileLocation.Occupier == null) {
			unitSelected.moveUnit (theVector, desiredTileLocation);
		// else, we've clicked on a Tile not inside of our moveRange
		} else {
			// TO DO: Relay message to player that move is not acceptable? 
		} 
	}

	// Calls Unit's moveUnit method to handle the movement of the Unit.
	public void moveUnit(Vector3 theVector, Tile newTile){
		unitSelected.moveUnit(theVector, newTile);

    }
		
	public bool isTileSelected() {
		return tileSelected != null;
	}

	public bool isUnitSelected() {
		return unitSelected != null;
	}

	public void changeUnitSelected(Unit newUnit) {
		// If we have a Unit selected already, disable its highlighting. 
		if (unitSelected != null) {
			toggleHighlightMovableTiles ();
		}

		// Change the selected unit to the given new unit.
		unitSelected = newUnit;

		// Toggle the highlighting of our new selected Unit if it's not null.
		if (newUnit != null) {
			toggleHighlightMovableTiles ();
		}
	}

	public Unit getUnitSelected() {
		return unitSelected;
	}

	public Tile getTileSelected() {
		return tileSelected;
	}

	// Changes the currently selected tile on the grid 
	public void changeTileSelected(Tile newSelected) {
		// if we have a tile selected, unhighlight the tiles highlighted for movement
		// and unhighlight the currently selected tile
		if (tileSelected != null) {
			tileSelected.toggleTileHighlight ();
			//toggleHighlightForMovement ();
		}

		// Set the new tile to be the currently selected.
		// Toggle the highlighting for its movement.
		tileSelected = newSelected;
		//toggleHighlightForMovement ();
	}

    // Method to toggle the highlight graphic for Tiles that our currently selected
	// Tile can visit. Includes highlighting of the Tile that the Unit is currently occupying. 
	public void toggleHighlightMovableTiles() {
		List<Tile> list = new List<Tile> ();
		explore (unitSelected.unitMoveRange, unitSelected.occupied, list);
		foreach (Tile t in list) {
			t.toggleTileHighlight ();
		}
	}

	// Helper method for finding Tiles that we can move to.
	//
	// Recursively explores Tiles in the Grid relative to the position of our
	// currently selected Unit's Tile (the position of the Tile the Unit is standing on). 
	private void explore(int moves, Tile curTile, List<Tile> list) {
		float dist = Vector3.Distance (curTile.transform.position, unitSelected.occupied.transform.position);
		if (dist <= unitSelected.unitMoveRange && moves >= 0) {
			if (!list.Contains (curTile)) { 
				list.Add (curTile);
			}

			if (curTile.NorthTile != null) {
				explore (moves - 1, curTile.NorthTile, list);
			}

			if (curTile.SouthTile != null) {
				explore (moves - 1, curTile.SouthTile, list);
			}

			if (curTile.EastTile != null) {
				explore (moves - 1, curTile.EastTile, list);
			}

			if (curTile.WestTile != null) {
				explore (moves - 1, curTile.WestTile, list);
			}
		}
	}
}
