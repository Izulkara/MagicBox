using UnityEngine;
using System.Collections;

public class AttackTest : MonoBehaviour {


    private Animator animator;
    public GameObject attackFX;
    private GameObject attack;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        //Attack();
        //Hurt();
        Death();
    }

    void Attack()
    {
        animator.SetTrigger("timeToAttack");
        Destroy(attack);
        attack = Instantiate(attackFX) as GameObject;
        attack.transform.position = new Vector3(0, 0, 15);
    }

    void Hurt()
    {
        animator.SetTrigger("timeToGetHurt");
    }

    void Death()
    {
        animator.SetTrigger("timeToGetHurt");
        animator.SetTrigger("timeToDie");
    }
}
