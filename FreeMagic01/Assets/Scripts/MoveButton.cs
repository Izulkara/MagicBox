using UnityEngine;
using System.Collections;

public class MoveButton : MonoBehaviour {

    public BattleManager theBattleManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        theBattleManager.canMove();
    }
}
