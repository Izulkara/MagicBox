using UnityEngine;
using System.Collections;

public class AttackButton : MonoBehaviour {

    Grid gridScript;

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
        gridScript.toggleHighlightAttackableTiles();
    }
}
