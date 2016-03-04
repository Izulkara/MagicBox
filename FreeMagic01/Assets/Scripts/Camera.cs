using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    public Vector3 myVector;
    public Vector3 targetVector;
    public float ratio;
    public float distance;

    // Use this for initialization
    void Start () {
        myVector = transform.position;
        targetVector = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (!myVector.Equals(targetVector))
        { //if we're not where we should be...
            ratio += Time.deltaTime; //then we should be closer to where we should be.
            transform.position = Vector3.Lerp(myVector, targetVector, (ratio / distance)); //dividing by the distance ensure the unit moves the same speed.
            if (transform.position.Equals(targetVector))
            {
                myVector = targetVector;
            }
        }
    }

    public void move (Vector3 theVector) {
        myVector = transform.position;
        float xdiff = theVector.x - myVector.x;
        float zdiff = theVector.z - myVector.z;
        xdiff = Mathf.Abs(xdiff);
        zdiff = Mathf.Abs(zdiff);
        distance = (xdiff + zdiff);
        targetVector = theVector;
        distance = distance / 4;
        ratio = 0;
    }
}
