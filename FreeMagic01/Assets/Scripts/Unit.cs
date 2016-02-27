using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
    public int id;
	public int unitHealth;
	public int unitMaxAttack;
	public int unitMinAttack;
	public bool isInitialized;
	public Stack history;   //need to import Points on a 2d array for history of player's moves
	public int unitDefense;
	public int unitDodgeChance;
	public int unitCriticalStrikeChance;
    public int unitMoveRange;
    public Vector3 myVector;
    public Vector3 targetVector;
    public float ratio;
    public float distance;
    public Tile occupied;
    public BattleManager theBattleManager;

	public Unit(int identification) 
		: this (identification, 100, 5, 10, 1, 5, 5, 3) {
	}

	public Unit(int identification, int health, int minAttack, int maxAttack, int defense, int dodgeChance, int criticalStrikeChance, int moveRange) {
        id = identification;
		isInitialized = true;
		unitHealth = health;
		unitMinAttack = minAttack;
		unitMaxAttack = maxAttack;
		unitDefense = defense;
		unitDodgeChance = dodgeChance;
		unitCriticalStrikeChance = criticalStrikeChance;
		history = new Stack();
        unitMoveRange = moveRange;
        myVector = transform.position;// used for positioning.
        targetVector = transform.position;
        ratio = 0; //used for movement.
    }

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update(){
        if (!myVector.Equals(targetVector)){ //if we're not where we should be...
            ratio += Time.deltaTime; //then we should be closer to where we should be.
            transform.position = Vector3.Lerp(myVector, targetVector, (ratio / distance)); //dividing by the distance ensure the unit moves the same speed.
        }  // Square rooted the speed at which it moves by dividing it by itself as well.
    }

    //clicking the unit causes it to move, albeit randomly.
    void OnMouseDown(){
        //theBattleManager.setUnit(id);
        //System.Random randomMove = new System.Random();
        //float xrand = randomMove.Next(3, 8);
        //float zrand = randomMove.Next(3, 8);
        //this.Move(new Vector3(xrand, 1, zrand));
    }
    //Plots the course for moving.
    public void moveUnit(Vector3 theVector) {
        myVector = transform.position;
        float xdiff = theVector.x - myVector.x;
        float zdiff = theVector.z - myVector.z;
        xdiff = Mathf.Abs(xdiff);
        zdiff = Mathf.Abs(zdiff); 
        distance = (xdiff + zdiff);
        if (distance <= 3)
        {
            targetVector = theVector;
            distance = distance / 4;
        }
        ratio = 0;
    }

	void Attack(Unit unit) {
		System.Random randomAttack = new System.Random();
		int attackValue = randomAttack.Next(this.unitMinAttack, (this.unitMinAttack + 1));
		/**
		 * TODO
		 * Implement critical strike system.
		**/
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
}
