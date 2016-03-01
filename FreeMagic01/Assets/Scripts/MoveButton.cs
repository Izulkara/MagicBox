using UnityEngine;
using System.Collections;

public class MoveButton : MonoBehaviour {

    public BattleManager theBattleManager;

    public Grid gridScript;

	// Use this for initialization
	void Start () {

        GameObject grid = GameObject.Find("Grid");
        gridScript = grid.GetComponent<Grid>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        gridScript.toggleHighlightMovableTiles();
    }
}
