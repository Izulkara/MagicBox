using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public int teamID;
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
    Grid gridScript;
    private Animator animator;
    public GameObject attackFX;
    private GameObject attack;
    public bool hasAttackedOnThisTurn;
    public bool hasMovedOnThisTurn;
    public Health healthScript;
    public GameObject HealthBarR;
    public bool needsEffects;
    public Unit target;
    GameObject battleLog;
    public BattleLog log;

    public Unit(int identification)
        : this(identification, 100, 5, 10, 1, 5, 5, 3, 1)
    {
    }

    public Unit(int identification, int health, int minAttack, int maxAttack, int defense, int dodgeChance, int criticalStrikeChance, int moveRange, int attackRange)
    {
        teamID = identification;
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
        healthScript = HealthBarR.GetComponent<Health>();
        battleLog = GameObject.Find("BattleLog");
        log = battleLog.GetComponent<BattleLog>();

    }

    // Use this for initialization
    void Start()
    {
        needsEffects = false;
        hasAttackedOnThisTurn = false;
        hasMovedOnThisTurn = false;
        theBattleManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleManager>();
        gridScript = GameObject.Find("Grid").GetComponent<Grid>();
        animator = GetComponent<Animator>();
    }

    // Used for first time initialization only 
    void Awake()
    {
        myVector = transform.position;
        targetVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!myVector.Equals(targetVector))
        { //if we're not where we should be...
            ratio += Time.deltaTime; //then we should be closer to where we should be.
            transform.position = Vector3.Lerp(myVector, targetVector, (ratio / distance)); //dividing by the distance ensure the unit moves the same speed.
            if (transform.position.Equals(targetVector))
            {
                myVector = targetVector;
                if (needsEffects){
                    needsEffects = false;
                    Destroy(attack);
                    attack = Instantiate(attackFX, new Vector3(target.transform.position.x - 0.1F,
                        target.transform.position.y - 0.1F, target.transform.position.z), this.transform.rotation) as GameObject;
                }
            }
        }  // Square rooted the speed at which it moves by dividing it by itself as well.
    }

    // Clicking the unit should cause a highlight selection.
    // Once selected the unit should be deselected if clicked again.
    void OnMouseDown()
    {
        // Check if it is our turn
        if (theBattleManager.isPlayerTurn)
        {
            // allow selection
            runSelection();
        }
    }

    //Plots the course for moving.
    public void moveUnit(Vector3 theVector, Tile newOccupied)
    {
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
        deselectAfterMovement();

        // Updates the Unit's field for the Tile that it is occupying. 
        occupied = newOccupied;

        hasMovedOnThisTurn = true;
    }

    public void moveEnemyUnit(Vector3 theVector, Tile newOccupied)
    {
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

        // Updates the Unit's field for the Tile that it is occupying. 
        occupied = newOccupied;
    }

    public void EnemyAttack(Unit unit)
    {
        target = unit;
        System.Random randomAttack = new System.Random();
        int attackValue = randomAttack.Next(this.unitMinAttack, this.unitMaxAttack);
        bool isCrit = false;
        int critResult = Random.Range(0, 100);
        if (critResult <= this.unitCriticalStrikeChance)
        {
            attackValue = attackValue * 2;
            isCrit = true;
        }

        needsEffects = true;
        unit.isAttacked(attackValue);
		HealthBarR.GetComponent<Health> ().updateHealthBar (this);
        //
        this.Update();
        animator.SetTrigger("timeToAttack");
        log.updateAttack(this, unit, attackValue, unit.unitDefense, isCrit);
    }

    public void Attack(Unit unit)
    {
        bool isCrit = false;
        System.Random randomAttack = new System.Random();
        int attackValue = randomAttack.Next(this.unitMinAttack, this.unitMaxAttack);

        int critResult = Random.Range(0, 100);
        if(critResult <= this.unitCriticalStrikeChance)
        {
            attackValue = attackValue * 2;
            isCrit = true;
        }

        deselectAfterAttack();
        animator.SetTrigger("timeToAttack");
        Destroy(attack);
        attack = Instantiate(attackFX, new Vector3(unit.transform.position.x - 0.1F, unit.transform.position.y - 0.1F, unit.transform.position.z), this.transform.rotation) as GameObject;
        unit.isAttacked(attackValue);
		HealthBarR.GetComponent<Health> ().updateHealthBar (unit);
        hasAttackedOnThisTurn = true;
        this.Update();
        log.updateAttack(this, unit, attackValue, unit.unitDefense, isCrit);
    }

    void UpdateHistory()
    {
        //history.Push (Point);
    }
    /*
	Point getLastMove() {
		history.Peek()
	}*/
    void isAttacked(int attackValue)
    {
        attackValue = attackValue - this.unitDefense;

        int dodgeResult = Random.Range(0, 100);
        if (dodgeResult <= this.unitDodgeChance)
        {
            attackValue = 0;
            log.updateDodge(this);
        }

        if (attackValue > 0)
        {
            animator.SetTrigger("timeToGetHurt");
        }
        this.unitHealth = this.unitHealth - attackValue;
        
        if (theBattleManager.enemyUnits.Contains(this))
        {
            if (this.unitHealth <= 0)
            {
                theBattleManager.enemyUnits.Remove(this);
                animator.SetTrigger("timeToDie");
                teamID = -1;
                //Destroy(this.gameObject);
                HealthBarR.GetComponent<Health>().updateHealthBar(this); // Leslie added to ensure that healthbar updates when attacked - even if game over
                checkGameOver();
            }
        }
        if (theBattleManager.friendlyUnits.Contains(this))
        {
            if (this.unitHealth <= 0)
            {
                theBattleManager.friendlyUnits.Remove(this);
                //Destroy(this.gameObject);
                animator.SetTrigger("timeToDie");
                teamID = -1;
                HealthBarR.GetComponent<Health>().updateHealthBar(this); // Leslie added to ensure that healthbar updates when attacked - even if game over
                checkGameOver();
            }
        }
        if (attackValue > 0)
        {
            animator.SetTrigger("timeToGetHurt");
        }

    }

    // Private helper method that handles unit selection cases. 
    private void selectionHelper()
    {
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
                gridScript.deselectUnit();
            }
            // Else, no unit is selected so select the unit and change the highlight 
        }
        else {
            gridScript.changeUnitSelected(this);
            this.GetComponent<SpriteRenderer>().material.color = Color.green;
        }
    }

    // Helper method to deselect a Unit after a movement has completed. 
    private void deselectAfterMovement()
    {
        gridScript.toggleHighlightMovableTiles();
        gridScript.moving = false;
        //gridScript.getUnitSelected ().GetComponent<SpriteRenderer> ().material.color = Color.white;
        //gridScript.changeUnitSelected (null);
    }

    private void runSelection()
    {
        if (gridScript.attacking)
        {
            gridScript.attemptAttack(myVector, occupied);
        }
        else if (gridScript.moving)
        {
            deselectAfterMovement();
        }
        else {
            // Else handles the selection code.

            // Only allow selection if it is on the friendly team.
            if (this.teamID == 1 || this.teamID == 0)
            {
                selectionHelper();
            }
        }
    }

    // Helper method to deselect the tiles after a unit has attacked.
    private void deselectAfterAttack()
    {
        gridScript.toggleHighlightAttackableTiles();
    }

    private void checkGameOver()
    {
        if (theBattleManager.enemyUnits.Count == 0)
        {
            theBattleManager.GetComponent<pauseMenuScript>().WinGame();
            gridScript.endTurnButton.SetActive(false);
        }

        if (theBattleManager.friendlyUnits.Count == 0)
        {
            theBattleManager.GetComponent<pauseMenuScript>().LoseGame();
            gridScript.endTurnButton.SetActive(false);
        }
    }

    public void setRotation(Quaternion theRotation)
    {
        transform.rotation = theRotation; // used to change camera angles.
    }
}
