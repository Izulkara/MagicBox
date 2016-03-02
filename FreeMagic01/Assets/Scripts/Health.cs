using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	//public float max_health = 200f;
	//public float curr_health = 0f;
	public GameObject healthBar;
    public GameObject grid;
    public Grid gridScript;
    public Vector3 fullHealth;

	// Use this for initialization
	void Start () {

        fullHealth = transform.localScale;
        grid = GameObject.Find("Grid");
        gridScript = grid.GetComponent<Grid>();
        //curr_health = max_health;
		//InvokeRepeating ("DecHealth", 1f, 1f);
	}

    public void updateHealthBar()
    {
        transform.localScale = fullHealth;
        float hit;
        float max = gridScript.getUnitSelected().unitMaxHealth;
        float current = gridScript.getUnitSelected().unitHealth;
        hit = ((max - current) / max) * -1;
        transform.localScale += new Vector3(hit, 0);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

	// Decrements the player's healthbar representation by the amount
	// passed to the function. This amount should be a ratio of attack damage
	// to current health and should be between 0 and 1, where 1 represents 100% 
	// of the player's health to be taken away.
	//void DecHealth() {
		
		//transform.localScale += new Vector3(hit, 0, 0);
	//}
} 
