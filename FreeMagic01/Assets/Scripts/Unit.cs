using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
    public int id;
	public float unitHealth;
    public float unitMaxHealth;
	public int unitMaxAttack;
	public int unitMinAttack;
	public bool isInitialized;
	public Stack history;   //need to import Points on a 2d array for history of player's moves
	public int unitDefense;
	public int unitDodgeChance;
	public int unitCriticalStrikeChance;
    public int unitMoveRange;
    public int unitAttackRange;
    public Vector3 myVector;
    public Vector3 targetVector;
    public float ratio;
    public float distance;
    public Tile occupied;
    public BattleManager theBattleManager;
    GameObject grid;
    Grid gridScript;
    private Animator animator;
    public GameObject attackFX;
    private GameObject attack;


    public Unit(int identification) 
		: this (identification, 100, 5, 10, 1, 5, 5, 3, 1) {
	}

	public Unit(int identification, int health, int minAttack, int maxAttack, int defense, int dodgeChance, int criticalStrikeChance, int moveRange, int attackRange) {
        id = identification;
		isInitialized = true;
		unitHealth = health;
        unitMaxHealth = health;
		unitMinAttack = minAttack;
		unitMaxAttack = maxAttack;
		unitDefense = defense;
		unitDodgeChance = dodgeChance;
		unitCriticalStrikeChance = criticalStrikeChance;
		history = new Stack();
        unitMoveRange = moveRange;
        unitAttackRange = attackRange;
        myVector = transform.position;// used for positioning.
        targetVector = transform.position;
        ratio = 0; //used for movement.
    }

	// Use this for initialization
	void Start () {

        grid = GameObject.Find("Grid");
        gridScript = grid.GetComponent<Grid>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        if (!myVector.Equals(targetVector)){ //if we're not where we should be...
            ratio += Time.deltaTime; //then we should be closer to where we should be.
            transform.position = Vector3.Lerp(myVector, targetVector, (ratio / distance)); //dividing by the distance ensure the unit moves the same speed.
			if (transform.position.Equals (targetVector)) {
				myVector = targetVector;
			}
        }  // Square rooted the speed at which it moves by dividing it by itself as well.
    }

    // Clicking the unit should cause a highlight selection.
    // Once selected the unit should be deselected if clicked again.
    void OnMouseDown()
    {
        // Get access to the Grid


        if (gridScript.attacking) {
            gridScript.attemptAttack(myVector, occupied);
        } else if (gridScript.moving){
            deselectAfterMovement();
        } else { 
            // If the a unit is selected, unhighlight and change the unit selected.
            if (gridScript.isUnitSelected())
            {
                // Grab the unit currently selected from the Grid
                Unit curSelected = gridScript.getUnitSelected();
                // Unhighlight the currently selected unit
                curSelected.GetComponent<SpriteRenderer>().material.color = Color.white;

                // If the new unit selected is not the unit currently selected, select it and highlight it
                if (!curSelected.Equals(this))
                {
                    gridScript.changeUnitSelected(this);
                    this.GetComponent<SpriteRenderer>().material.color = Color.green;
                    // Else, we've clicked on the same unit so deselect 
                }
                else {
                    gridScript.changeUnitSelected(null);
                }
                // Else, no unit is selected so select the unit and change the highlight 
            }
            else {
                gridScript.changeUnitSelected(this);
                this.GetComponent<SpriteRenderer>().material.color = Color.green;
            }
        }
    }
		
    //Plots the course for moving.
	public void moveUnit(Vector3 theVector, Tile newOccupied) {
		myVector = transform.position;
        float xdiff = theVector.x - myVector.x;
        float zdiff = theVector.z - myVector.z;
        xdiff = Mathf.Abs(xdiff);
        zdiff = Mathf.Abs(zdiff); 
        distance = (xdiff + zdiff);
        targetVector = theVector;
        distance = distance / 4;
        ratio = 0;

		// Updates the Tile occupier fields. 
		newOccupied.Occupier = this;
		occupied.Occupier = null;

		// Deselects the Unit after its movement is complete
		// Comment out the following line to retain selection after movement
		deselectAfterMovement ();

		// Updates the Unit's field for the Tile that it is occupying. 
		occupied = newOccupied;
    }

	public void Attack(Unit unit) {
		System.Random randomAttack = new System.Random();
		int attackValue = randomAttack.Next(this.unitMinAttack, (this.unitMinAttack + 1));
        /**
		 * TODO
		 * Implement critical strike system.
		**/
        deselectAfterAttack();
        animator.SetTrigger("timeToAttack");
        unit.isAttacked(attackValue);
		this.Update();
	}

	void UpdateHistory() {
		//history.Push (Point);
	}
	/*
	Point getLastMove() {
		history.Peek()
	}*/
	void isAttacked(int attackValue) {
		//TODO 
		/**
		 * Implement dodging system.
		**/
		attackValue = attackValue - this.unitDefense;
		this.unitHealth = this.unitHealth - attackValue;
	} 

	// Helper method to deselect a Unit after a movement has completed. 
	private void deselectAfterMovement() {
        gridScript.toggleHighlightMovableTiles();
        gridScript.moving = false;
		//gridScript.getUnitSelected ().GetComponent<SpriteRenderer> ().material.color = Color.white;
		//gridScript.changeUnitSelected (null);
	}

    // Helper method to deselect the tiles after a unit has attacked.
    private void deselectAfterAttack(){
        gridScript.toggleHighlightAttackableTiles();
    }
}
