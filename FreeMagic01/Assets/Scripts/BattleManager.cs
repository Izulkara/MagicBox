using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {

    public Unit selectedUnit;
    public Tile selectedTile;
    public Grid theGrid;
    public SortedList unitIds;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void moveUnit(Vector3 theVector){
        selectedUnit.moveUnit(theVector);
    }

    public void canMove(){

    }

    public void setUnit(theId)
    {
        selectedUnit = 
    }

    public void setTile()
    {

    }
}
