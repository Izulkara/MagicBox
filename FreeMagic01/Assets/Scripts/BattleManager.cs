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
			Tile t = findMovableTile (u);

			if (t.Occupier == null) {
				u.moveEnemyUnit (new Vector3 (t.transform.position.x, t.height, t.transform.position.z), t);
			} else {
				Unit occupied = t.Occupier;
				if (occupied.teamID == 1) {
					u.EnemyAttack(occupied);
				}
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

    
}
