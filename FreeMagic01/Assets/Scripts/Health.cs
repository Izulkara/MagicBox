using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float max_health = 200f;
	public float curr_health = 0f;

	// Use this for initialization
	void Start () {
		curr_health = max_health;
		InvokeRepeating ("DecHealth", 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void DecHealth() {
		curr_health -= 20f;
	}
} 
