using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BattleManager : MonoBehaviour {

	public static System.Random RNG = new System.Random();

    public Unit selectedUnit;
    public Tile selectedTile;
    public Grid theGrid;
	public List<Unit> enemyUnits;
	public List<Unit> friendlyUnits;
	public bool isPlayerTurn;
    public float ratio;

	// Use this for initialization
	void Start () {
		isPlayerTurn = true;
		enemyUnits = new List<Unit> ();
		friendlyUnits = new List<Unit> ();

		Unit[] findUnits = GameObject.FindObjectsOfType<Unit> ();
		for (int x = 0; x < findUnits.Length; x++) {
			if (findUnits[x].teamID == 0) {
				enemyUnits.Add (findUnits [x]);
			} else {
				friendlyUnits.Add (findUnits [x]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

    }

	public void endPlayerTurn() {
        if(theGrid.isUnitSelected())
        {
            theGrid.deselectUnit();
        }
		isPlayerTurn = false;
		executeAITurn ();
		foreach (Unit u in friendlyUnits) {
			u.hasMovedOnThisTurn = false;
			u.hasAttackedOnThisTurn = false;
		}
		isPlayerTurn = true;
	}

	private void executeAITurn() {
		foreach (Unit u in enemyUnits) {
            // Begin Movement
            Unit target = findClosestUnit(u);
            List<Tile> list = new List<Tile>();
            explore(u, u.unitMoveRange, u.occupied, list);

            float maxDistance = 99;
            Tile moveToThisTile = u.occupied;
            foreach(Tile t in list)
            {
                if (t.Occupier == null){

                    float distanceToTarget;
                    float x = target.myVector.x - t.transform.position.x;
                    float z = target.myVector.z - t.transform.position.z;
                    x = Mathf.Abs(x);
                    z = Mathf.Abs(z);
                    distanceToTarget = x + z;
                    if (distanceToTarget < maxDistance && distanceToTarget >= u.unitAttackRange)
                    {
                        moveToThisTile = t;
                        maxDistance = distanceToTarget;
                    }
                }
            }
            Vector3 movementVector = new Vector3(moveToThisTile.transform.position.x, moveToThisTile.height, moveToThisTile.transform.position.z);
            u.moveEnemyUnit(movementVector, moveToThisTile);

            // End Movement.
            
            
            if(maxDistance <= u.unitAttackRange)
            {
                u.EnemyAttack(target);
            }
		}

		isPlayerTurn = true;
	}

	private Tile findMovableTile(Unit u) {
		List<Tile> l = new List<Tile> ();
		if (u.occupied.NorthTile != null) {
			l.Add (u.occupied.NorthTile);
		}

		if (u.occupied.EastTile != null) {
			l.Add (u.occupied.EastTile);
		}

		if (u.occupied.SouthTile != null) {
			l.Add (u.occupied.SouthTile);
		}

		if (u.occupied.WestTile != null) {
			l.Add (u.occupied.WestTile);
		}

		return l[BattleManager.RNG.Next(0, l.Count)];
	}

    private Unit findClosestUnit(Unit currentUnit)
    {
        Unit closest = null;
        float shortestDistance = 99;
        foreach(Unit u in friendlyUnits)
        {
            float distance;
            float x = currentUnit.myVector.x - u.myVector.x;
            float z = currentUnit.myVector.z - u.myVector.z;
            x = Mathf.Abs(x);
            z = Mathf.Abs(z);
            distance = x + z;
            if(distance < shortestDistance)
            {
                closest = u;
                shortestDistance = distance;
            }
        }
        return closest;
    }

    private void explore(Unit u, int moves, Tile curTile, List<Tile> list)
    {
        float dist;
        float x = curTile.transform.position.x - u.occupied.transform.position.x;
        float z = curTile.transform.position.z - u.occupied.transform.position.z;
        x = Mathf.Abs(x);
        z = Mathf.Abs(z);
        dist = x + z;

        int range = u.unitMoveRange;

        if (dist <= range && moves >= 0)
        {
            if (!list.Contains(curTile))
            {
                list.Add(curTile);
            }

            if (curTile.NorthTile != null)
            {
                explore(u, moves - 1, curTile.NorthTile, list);
            }

            if (curTile.SouthTile != null)
            {
                explore(u, moves - 1, curTile.SouthTile, list);
            }

            if (curTile.EastTile != null)
            {
                explore(u, moves - 1, curTile.EastTile, list);
            }

            if (curTile.WestTile != null)
            {
                explore(u, moves - 1, curTile.WestTile, list);
            }
        }
    }

}
