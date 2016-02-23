using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

    bool Paused = false;

	// Use this for initialization
	void Start ()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (Paused == false)
            {
                Paused = true;
                Application.LoadLevel(1);
            }
            else
            {
                Paused = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (Paused == false)
            {
                Paused = true;
                Application.LoadLevel(1);
            }
            else
            {
                Paused = false;
            }
        }
    }
}
