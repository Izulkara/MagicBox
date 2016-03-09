﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // the left and right health panels
    public GameObject frameLeft;
    public GameObject frameRight;

    // the left and right health bars
    public GameObject healthBarLeft;
    public GameObject healthBarRight;
    public GameObject BckgrLeft;
    public GameObject BckgrRight;

    // the left and right ratio texts
    public Text healthRatioLeft;
    public Text healthRatioRight;

    // the left and right player names and damage/defense info
    public Text playerNameLeft;
    public Text playerNameRight;
	public Text playerDmg;
	public Text enemyDmg;
	public Text playerCrit;
	public Text enemyCrit;
	public Text playerDef;
	public Text enemyDef;
	public Text playerDodge;
	public Text enemyDodge;

    // the left portraits to display
    public GameObject archerPic;

    // game info
    public GameObject grid;
    public Grid gridScript;
    public Vector3 fullHealth;

    // Use this for initialization
    void Start() {
        // find all game objects
		findAllGameObjects();
        // get required components
		getComponentsNeeded();
        // save full health position
        fullHealth = transform.localScale;
		// set all texts at start state
		setStartText();
        
    }
	// Finds all the game objects needed.
	private void findAllGameObjects() {
		frameLeft = GameObject.Find("PortraitFrameL");
		frameRight = GameObject.Find("PortraitFrameR");
		healthBarLeft = GameObject.Find("HealthBarL");
		healthBarRight = GameObject.Find("HealthBarR");
		BckgrLeft = GameObject.Find("BackgroundL");
		BckgrRight = GameObject.Find("BackgroundR");
		grid = GameObject.Find("Grid");
		archerPic = GameObject.Find("ArcherPic");
	}

	// Gets the components from the game objects needed for this script.
	private void getComponentsNeeded() {
		healthRatioLeft = healthBarLeft.GetComponentInChildren<Text>();
		healthRatioRight = healthBarRight.GetComponentInChildren<Text>();
		playerNameLeft = frameLeft.GetComponentInChildren<Text>();
		playerNameRight = frameRight.GetComponentInChildren<Text>();
		gridScript = grid.GetComponent<Grid>();
	}

	//sets the text to all starting states
	private void setStartText() {
		BckgrLeft.GetComponentInChildren<Text>().enabled = false;
		BckgrRight.GetComponentInChildren<Text>().enabled = false;
		playerCrit.enabled = false;
		enemyCrit.enabled = false;
		playerDef.enabled = false;
		enemyDef.enabled = false;
		playerDodge.enabled = false;
		enemyDodge.enabled = false;

	}

    // called in Grid class
    // updates the appropriate healthbar to represent the currently selected unit,
    // their personal health, name, and image.
    public void updateHealthBar() {
		//playerDmg.enabled = true;
		//enemyDmg.enabled = true;
        updateHealth(gridScript.getUnitSelected());
		updateDmgText (gridScript.getUnitSelected());
    }

	// overload of above function. This one is called from
	// the Unit class to update healthbars in battle.
	public void updateHealthBar(Unit unit) {
		updateHealth(unit);
		updateDmgText (unit);
	}

	// updates the text indicating the dmg this unit can do
	public void updateDmgText(Unit unit) {
		String dmg = unit.unitMinAttack.ToString () + "-" + unit.unitMaxAttack.ToString ();
		String crit = unit.unitCriticalStrikeChance.ToString() + "%";
		String def = unit.unitDefense.ToString();
		String dodge = unit.unitDodgeChance.ToString() + "%";
		if (unit.name.Equals ("Steve") || unit.name.Equals ("John")) {
			playerDmg.text = "Attack Damage: " + dmg;
			playerDef.text = "Defense: " + def;
			playerCrit.text = "Critical Strike: " + crit;
			playerDodge.text = "Dodge: " + dodge;
		} else {
			enemyDmg.text = "Attack Damage: " + dmg;
			enemyDef.text = "Defense: " + def;
			enemyCrit.text = "Critical Strike: " + crit;
			enemyDodge.text = "Dodge: " + dodge;
		}
	}

    // called in the unit class when an enemy is attacked
    // the attacked unit is passed 
    public void updateEnemyHealthBar(Unit unit) {
        updateHealth(unit);
    }

    // make the necessary changes to the appropriate health bar
    public void updateHealth(Unit unit) {
        // hit value and unit variables
        float hit;
        bool isLeft = false;
        if (unit) { // get the selected unit and ensure it is not null
            float max = unit.unitMaxHealth; // get the max health
            float current = unit.unitHealth; // get the current health
				
            hit = ((max - current) / max) * -1; // calculate hit ratio
			if (current <= 0) { // make adjustments if the unit is newly dead
				hit = -1;
				current = 0;
			}
          if (unit.name.Equals("John") || unit.name.Equals("Steve")) { // if it is John or Steve - trigger left flag
                isLeft = true;
            }
            playerHealth(current, max, hit, isLeft); // to change the text on the health bar
            playerImageName(unit); // displays the unit's name on the health bar
        }
    }

    // displays the current health / max health on the appropriate health bar
    private void playerHealth(float current, float max, float hit, bool isLeft) {
        String ratio = current + "/" + max; // the current ratio for health
		if (isLeft) { // if isLeft is true then it is the left side to be updated
            healthRatioLeft.text = ratio; // the string for the healthbar
            if (current <= (max * .4)) { // if the bar is less than or equal to 40% full
                healthRatioLeft.enabled = false; //disable bar display
                BckgrLeft.GetComponentInChildren<Text>().enabled = true; // enable the text in background
                BckgrLeft.GetComponentInChildren<Text>().text = ratio; // display also on background
			} else if (!(healthRatioLeft.enabled) && current > (max * .4)) { // if the health has regenerated to above 40%
                healthRatioLeft.enabled = true; // enable bar text display
			}
			healthBarLeft.transform.localScale = fullHealth; // transform to full healthbar scale and...
			healthBarLeft.transform.localScale += new Vector3 (hit, 0); // ...move the health bar to new scale (position)
        } else { // else it is the right
            healthRatioRight.text = ratio; // the string for the healthbar
            if (current <= (max * .4)) { // if the bar is less than or equal to 40% full
                healthRatioRight.enabled = false; // disable bar text
                BckgrRight.GetComponentInChildren<Text>().enabled = true; // enable background text
                BckgrRight.GetComponentInChildren<Text>().text = ratio; // display also on background
            } else if (!healthRatioRight.enabled && current > (max * .4)) { // of the health has regenerated tp above 40%
                healthRatioRight.enabled = true; // enable the bar text display
			}
			healthBarRight.transform.localScale = fullHealth; // transform to full healthbar scale and...
			healthBarRight.transform.localScale += new Vector3 (hit, 0); // ...move the health bar to new scale (position)
        }
		 
    }
    // changes the image and name base on which unit is selected
    // Image currently not changing but name is
    // took renderer off of frame in unity and now not working. 
    // Still have not gotten images to change...
    private void playerImageName(Unit unit) {
        if (unit.name.Equals("Steve")) {
            archerPic.SetActive(true); //activate steve
            playerNameLeft.text = unit.name; // display the name
        } else if (unit.name.Equals("John")) {
            archerPic.SetActive(false); // hide steve
            playerNameLeft.text = unit.name; // display the name
        } else { // it is a Golem, same photo
            playerNameRight.text = unit.name; // display the name
        }
    }

    // Update is called once per frame
    void Update()
    {
		//if (Unit u = gridScript.getUnitSelected ()) {
		//	updateHealth(u);
		//}
    }

}
