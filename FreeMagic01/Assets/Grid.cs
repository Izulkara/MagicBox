using UnityEngine;
using System.Collections;


public class Grid : MonoBehaviour
{
    private Tile[,] tiles2D;

	// Use this for initialization
	void Start ()
    {
        tiles2D = new Tile[10, 10];
        Tile[] tiles = gameObject.GetComponentsInChildren<Tile>();

        foreach (Tile t in tiles)
        {
            tiles2D[(int)t.transform.localPosition.x,(int)t.transform.localPosition.z] = t;
            
        }

        
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }
}
