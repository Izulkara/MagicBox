using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class BattleLog : MonoBehaviour {

	public Text log;

	public int maxSize;

	// Use this for initialization
	void Start () {
		maxSize = 2000; // might change
		log.text = "Welcome to Free Magic! Enjoy your journey... Bwa ha ha \n"; // start text
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void addText(String data) {
		log.text += data;
		// if the string is growing too large, delete a line.
		if ( log.text.Length>= maxSize) {
			int line = log.text.IndexOf ('\n') + 1;
			log.text = log.text.Substring (line);
		}
	}
		

	public void updateAttack(Unit attacker, Unit victim, int damage, int defense, bool crit) {
		String s;
		s = attacker.name + " attacks " + victim.name + " for " + damage + " (-" + defense +" DEFENSE)" ;
		if (crit) {
			s += "<CRITICAL STRIKE>";
		}
		s += "\n";
		addText (s);
	}

	public void updateDodge(Unit victim) {
		String s;
		s = victim.name + " DODGED! No damage taken \n";
		addText (s);
	}
}
