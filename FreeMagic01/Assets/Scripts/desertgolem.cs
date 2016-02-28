using UnityEngine;
using System.Collections;

public class desertgolem : MonoBehaviour
{

    private int i;
    private int j;
    private int k;
    private bool l = true;

    // Use this for initialization
    void Start()
    {
        i = 0;
        j = 1;
        k = 0;
        //GUILayout.Label("rawr");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);

        k++;
        if (k > 12)
        {
            k = 0;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        if (l)
        {
            i++;
            if (i > 30)
            {
                i = 0;
                j *= -1;
            }
            float h = 0.1F;
            h *= j;
            transform.position = new Vector3(transform.position.x + h, transform.position.y, transform.position.z);
        }
    }
    /*
    void OnClick() {
        l = !l;
    }
    */

    void OnMouseDown() {
        //l = false;
        l = !l;
    }

    public void StopAGaben()
    {
        l = !l;
    }
}
