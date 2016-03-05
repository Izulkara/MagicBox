using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	private Tile tileSelected;
	private Unit unitSelected;
    public BattleManager theBattleManager;
    public bool moving = false;
    public bool attacking = false;
    GameObject moveButton;
    GameObject attackButton;
    GameObject waitButton;
    GameObject HealthBar;
    GameObject CameraTarget;
    GameObject HealthBarL;
    new Camera camera;
    Health healthScript;
    Tile[,] tiles2D;

    // Use this for initialization
    void Start ()
    {
        moveButton = GameObject.Find("MoveButton");
        attackButton = GameObject.Find("AttackButton");
        waitButton = GameObject.Find("WaitButton");
        HealthBarL = GameObject.Find("HealthBarL");
        CameraTarget = GameObject.Find("CameraTarget");
        healthScript = HealthBarL.GetComponent<Health>();
        camera = CameraTarget.GetComponent<Camera>();
        initializeGrid();
    }

    private void initializeGrid()
    {
        tiles2D = new Tile[10, 10];
        Tile[] tilesToProcess = gameObject.GetComponentsInChildren<Tile>();
        GameObject units = GameObject.Find("Units");
        Unit[] unitsToProcess = units.GetComponentsInChildren<Unit>();

        print(unitsToProcess[0] + "  Good");

        foreach (Tile t in tilesToProcess)
        {
            tiles2D[(int)t.transform.localPosition.x, (int)t.transform.localPosition.z] = t;
        }



        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                if (tiles2D[x, z] != null)
                {
                    int north = z + 1;
                    int south = z - 1;
                    int east = x + 1;
                    int west = x - 1;

                    if (north >= 0 && north <= 9 && tiles2D[x, north] != null)
                    {
                        tiles2D[x, z].NorthTile = tiles2D[x, north];
                    }
                    if (south >= 0 && south <= 9 && tiles2D[x, south] != null)
                    {
                        tiles2D[x, z].SouthTile = tiles2D[x, south];
                    }
                    if (west >= 0 && west <= 9 && (tiles2D[west, z] != null))
                    {
                        tiles2D[x, z].WestTile = tiles2D[west, z];
                    }
                    if (east >= 0 && east <= 9 && (tiles2D[east, z] != null))
                    {
                        tiles2D[x, z].EastTile = tiles2D[east, z];
                    }
                }

            }
        }

        foreach (Unit u in unitsToProcess)
        {
            int unitX = (int)u.transform.localPosition.x;
            int unitZ = (int)u.transform.localPosition.z;

            u.occupied = tiles2D[unitX, unitZ];
            tiles2D[unitX, unitZ].Occupier = u;

        }
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
		if (distance <= unitSelected.unitMoveRange & desiredTileLocation.Occupier == null & moving) {
			unitSelected.moveUnit (theVector, desiredTileLocation);
            moveButton.SetActive(false);
            // else, we've clicked on a Tile not inside of our moveRange
        } else {
			// TO DO: Relay message to player that move is not acceptable? 
		} 
	}


    public void attemptAttack(Vector3 theVector, Tile theTile) {
        float distance = Vector3.Distance(unitSelected.myVector, theVector);

        if(distance <= unitSelected.unitAttackRange & theTile.Occupier != null)
        {
            unitSelected.Attack(theTile.Occupier);
            attackButton.SetActive(false);
        }
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
			//toggleHighlightMovableTiles ();
		}

		// Change the selected unit to the given new unit.
		unitSelected = newUnit;
        attackButton.SetActive(true);
        moveButton.SetActive(true);
        waitButton.SetActive(true);
        healthScript.updateHealthBar();


        // Toggle the highlighting of our new selected Unit if it's not null.
        if (newUnit != null) {
			//toggleHighlightMovableTiles ();
		}
	}

    //used by wait button to 'end turn'
    public void deselectUnit(){
        if (moving) toggleHighlightMovableTiles();
        if (attacking) toggleHighlightAttackableTiles();
        attackButton.SetActive(false);
        moveButton.SetActive(false);
        waitButton.SetActive(false);
        unitSelected.GetComponent<SpriteRenderer>().material.color = Color.white;
        unitSelected = null;
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
        //if (tileSelected != null) {
        //tileSelected.toggleTileHighlight ("Teal");
        //toggleHighlightForMovement ();
        //}
        Vector3 tileVector = new Vector3(newSelected.transform.position.x, 1, newSelected.transform.position.z);
        camera.move(tileVector);

		// Set the new tile to be the currently selected.
		// Toggle the highlighting for its movement.
		tileSelected = newSelected;
		//toggleHighlightForMovement ();
	}

    // Method to toggle the highlight graphic for Tiles that our currently selected
	// Tile can visit. Includes highlighting of the Tile that the Unit is currently occupying. 
	public void toggleHighlightMovableTiles() {
        if (attacking) toggleHighlightAttackableTiles();
        moving = !moving; //toggles boolean for moving.
        List<Tile> list = new List<Tile> ();
		explore (unitSelected.unitMoveRange, unitSelected.occupied, list);
		foreach (Tile t in list) {
			t.toggleTileHighlight("Teal");
		}
        
	}

    // Method to toggle the highlight graphic for Tiles that our currently selected
    // Tile can visit. Includes highlighting of the Tile that the Unit is currently occupying. 
    public void toggleHighlightAttackableTiles(){
        if (moving) toggleHighlightMovableTiles();
        bool wasAttacking = false;
        if (!attacking) attacking = !attacking; //if we weren't attacking, we are now.
        else wasAttacking = true; //if we were attacking we should remember that.
        List<Tile> list = new List<Tile>();
        explore(unitSelected.unitAttackRange, unitSelected.occupied, list);
        foreach (Tile t in list) {
            t.toggleTileHighlight("Orange");
        }
        if (wasAttacking) attacking = false; //if we were attacking we should no longer be.
        
        
    }

    // Helper method for finding Tiles that we can move to.
    //
    // Recursively explores Tiles in the Grid relative to the position of our
    // currently selected Unit's Tile (the position of the Tile the Unit is standing on). 
    private void explore(int moves, Tile curTile, List<Tile> list) {
		float dist = Vector3.Distance (curTile.transform.position, unitSelected.occupied.transform.position);
        int range;
        if (attacking) range = unitSelected.unitAttackRange;
        else range = unitSelected.unitMoveRange;

		if (dist <= range && moves >= 0) {
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
