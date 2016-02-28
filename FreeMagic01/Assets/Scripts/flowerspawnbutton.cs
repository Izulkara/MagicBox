using UnityEngine;
using System.Collections;

public class flowerspawnbutton : MonoBehaviour {

    public GameObject m_enemy;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        GameObject desertgolem = Instantiate(m_enemy) as GameObject;
        desertgolem.transform.position = new Vector3(0, 0, 5);
    }
}
