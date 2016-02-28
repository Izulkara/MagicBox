using UnityEngine;
using System.Collections;

public class lumberattacktest : MonoBehaviour {

    private Animator animator;
    public GameObject attackFX;
    private GameObject attack;
    private bool attackSpawned;

    // Use this for initialization
    void Start()
    {
        attackSpawned = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetTrigger("playerAttack");
        if (attackSpawned)
        {
            attack.transform.position = new Vector3(attack.transform.position.x - 0.1F, attack.transform.position.y, attack.transform.position.z);
        }
    }

    void OnMouseDown()
    {
        attackSpawned = true;
        animator.SetTrigger("timeToAttack");
        //attack.SetActive(false);
        Destroy(attack);
        attack = Instantiate(attackFX) as GameObject;
        //attack.SetActive(true);
        attack.transform.position = new Vector3(0, 0, 15);
        //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
