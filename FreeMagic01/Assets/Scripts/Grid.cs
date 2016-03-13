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
    public GameObject endTurnButton;
    GameObject HealthBar;
    GameObject CameraTarget;
    GameObject HealthBarL;
    new Camera camera;
    Health healthScript;

    // Use this for initialization
    void Start ()
    {
        moveButton = GameObject.Find("MoveButton");
        attackButton = GameObject.Find("AttackButton");
        waitButton = GameObject.Find("WaitButton");
        endTurnButton = GameObject.Find("EndTurnButton");
        HealthBarL = GameObject.Find("HealthBarL");
        CameraTarget = GameObject.Find("CameraTarget");
        healthScript = HealthBarL.GetComponent<Health>();
        camera = CameraTarget.GetComponent<Camera>();
        initializeGrid();
        attackButton.SetActive(false);
        moveButton.SetActive(false);
        waitButton.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    //initializeGrid is used for establishing the tile network.
    private void initializeGrid()
    {

        int mapSize = 50;
        Tile[,] tiles2D = new Tile[mapSize, mapSize];
        Tile[] tilesToProcess = gameObject.GetComponentsInChildren<Tile>();
        GameObject unitsObject = GameObject.Find("Units");
        Unit[] units = unitsObject.GetComponentsInChildren<Unit>();

        foreach (Tile t in tilesToProcess)
        {
            t.Occupier = null;
            tiles2D[(int)t.transform.localPosition.x, (int)t.transform.localPosition.z] = t;
        }

        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                if (tiles2D[x, z] != null)
                {
                    int north = z + 1;
                    int south = z - 1;
                    int east = x + 1;
                    int west = x - 1;

                    if (north >= 0 && north <= (mapSize -1) && tiles2D[x, north] != null)
                    {
                        tiles2D[x, z].NorthTile = tiles2D[x, north];
                    }
                    if (south >= 0 && south <= (mapSize - 1) && tiles2D[x, south] != null)
                    {
                        tiles2D[x, z].SouthTile = tiles2D[x, south];
                    }
                    if (west >= 0 && west <= (mapSize - 1) && (tiles2D[west, z] != null))
                    {
                        tiles2D[x, z].WestTile = tiles2D[west, z];
                    }
                    if (east >= 0 && east <= (mapSize - 1) && (tiles2D[east, z] != null))
                    {
                        //print(x + " " + z);
                        tiles2D[x, z].EastTile = tiles2D[east, z];
                    }
                }
            }
        }
        foreach (Unit u in units)
        {
            tiles2D[(int)u.myVector.x, (int)u.myVector.z].Occupier = u;
            u.occupied = tiles2D[(int)u.myVector.x, (int)u.myVector.z];

        }
    }

    // Attempts to move a Unit to the given desiredTileLocation.
    // 
    // Determines whether the Unit can be moved based on the distance of the Unit's Tile position
    // relative to the desiredTileLocation's position. If the distance is less than or equal to the 
    // Unit's move range then the move is executed. 
    public void attemptMove(Vector3 theVector, Tile desiredTileLocation) {
        float distance;
        float x = unitSelected.myVector.x - theVector.x;
        float z = unitSelected.myVector.z - theVector.z;
        x = Mathf.Abs(x);
        z = Mathf.Abs(z);
        distance = x + z;
        if (unitSelected.teamID == 0)
        {

        }
        // if the distance of the Tile we clicked on is less than or equal to our moveRange, then move
        else if (desiredTileLocation.TileHighlight.activeSelf && moving && desiredTileLocation.Occupier == null)
        {
			unitSelected.moveUnit (theVector, desiredTileLocation);
            moveButton.SetActive(false);
            // else, we've clicked on a Tile not inside of our moveRange
        } else {
			// TO DO: Relay message to player that move is not acceptable? 
		} 
	}


    public void attemptAttack(Vector3 theVector, Tile theTile) {
        float distance;
        float x = unitSelected.myVector.x - theVector.x;
        float z = unitSelected.myVector.z - theVector.z;
        x = Mathf.Abs(x);
        z = Mathf.Abs(z);
        distance = x + z;

        if(unitSelected.teamID == 0)
        {

        }
        else if (theTile.TileHighlight.activeSelf && theTile.Occupier.teamID == 0)
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
        attackButton.SetActive(!unitSelected.hasAttackedOnThisTurn);
        moveButton.SetActive(!unitSelected.hasMovedOnThisTurn);
        waitButton.SetActive(true);
        healthScript.updateHealthBar();
        camera.move(newUnit.myVector);

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

        if(!this.isUnitSelected())
        {
            camera.move(tileVector);
        }
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
    private void explore(int moves, Tile curTile, List<Tile> list)
    {
        float dist;
        float x = curTile.transform.position.x - unitSelected.occupied.transform.position.x;
        float z = curTile.transform.position.z - unitSelected.occupied.transform.position.z;
        x = Mathf.Abs(x);
        z = Mathf.Abs(z);
        dist = x + z;

        int range;
        if (attacking)
        {
            range = unitSelected.unitAttackRange;
            if (dist <= range && moves >= 0)
            {
                if (!list.Contains(curTile))
                {
                    list.Add(curTile);
                }

                if (curTile.NorthTile != null)
                {
                    explore(moves - 1, curTile.NorthTile, list);
                }

                if (curTile.SouthTile != null)
                {
                    explore(moves - 1, curTile.SouthTile, list);
                }

                if (curTile.EastTile != null)
                {
                    explore(moves - 1, curTile.EastTile, list);
                }

                if (curTile.WestTile != null)
                {
                    explore(moves - 1, curTile.WestTile, list);
                }
            }
        }
        else
        {
            range = unitSelected.unitMoveRange;

            if (dist <= range && moves >= 0)
            {
                if (!list.Contains(curTile))
                {
                    list.Add(curTile);
                }

                if (curTile.NorthTile != null && (curTile.NorthTile.Occupier == null || curTile.NorthTile.Occupier.teamID == 1))
                {
                    explore(moves - 1, curTile.NorthTile, list);
                }

                if (curTile.SouthTile != null && (curTile.SouthTile.Occupier == null || curTile.SouthTile.Occupier.teamID == 1))
                {
                    explore(moves - 1, curTile.SouthTile, list);
                }

                if (curTile.EastTile != null && (curTile.EastTile.Occupier == null || curTile.EastTile.Occupier.teamID == 1))
                {
                    explore(moves - 1, curTile.EastTile, list);
                }

                if (curTile.WestTile != null && (curTile.WestTile.Occupier == null || curTile.WestTile.Occupier.teamID == 1))
                {
                    explore(moves - 1, curTile.WestTile, list);
                }
            }
        }
    }


}
