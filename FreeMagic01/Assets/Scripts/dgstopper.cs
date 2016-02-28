using UnityEngine;
using System.Collections;

public class dgstopper : MonoBehaviour
{

    //public GameObject m_enemy;
    public desertgolem gorem;
    private desertgolem badguy;

    // Use this for initialization
    void Start()
    {
        badguy = Instantiate(gorem) as desertgolem;
        badguy.transform.position = new Vector3(0, 0, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        //desertgolem badguy = Instantiate(gorem) as desertgolem;
        //badguy.transform.position = new Vector3(0, 0, 5);
        badguy.StopAGaben();
    }
    
}
