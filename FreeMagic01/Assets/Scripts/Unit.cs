using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
	public int unitHealth;
	public int unitMaxAttack;
	public int unitMinAttack;
	public bool isInitialized;
	public Stack history;   //need to import Points on a 2d array for history of player's moves
	public int unitDefense;
	public int unitDodgeChance;
	public int unitCriticalStrikeChance;

	public Unit() 
		: this (100, 5, 10, 1, 5) {
	}

	public Unit(int health, int minAttack, int maxAttack, int defense, int dodgeChance, int criticalStrikeChance) {
		isInitialized = true;
		unitHealth = health;
		unitMinAttack = minAttack;
		unitMaxAttack = maxAttack;
		unitDefense = defense;
		unitDodgeChance = dodgeChance;
		unitCriticalStrikeChance = criticalStrikeChance;
		history = new Stack();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//animation updates
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
