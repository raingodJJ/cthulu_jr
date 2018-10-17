using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentEnemy : MonoBehaviour {


    public float hitsRemain = 3f;

    public float speed = 1f;

    public int damage = 5;

    public float meleeRange = 1f;

    public GameObject player;

    public float MinDist = 0f;

    public float MaxDist = 0f;

    public float attackRate = 1f;

    private float interval = 0f;

    private bool attacking = false;



	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
    }

 

    // Update is called once per frame
    void Update() {
        /*
         * if (hitsRemain <= 0)
         * {
         *  Die();
         * }
         */


        transform.LookAt(player.transform);

        if (Vector3.Distance(transform.position, player.transform.position) >= MinDist)
        {

            transform.position += transform.forward * speed * Time.deltaTime;


            if (Vector3.Distance(transform.position, player.transform.position) <= MaxDist)
            {
                Invoke("DealDamage", 1f);                
            }

        }
    }

    void DealDamage()
    {
        if(attacking)
        {
            return;
        }
        attacking = true;
        if(Vector3.Distance(transform.position, player.transform.position) >= meleeRange)
        {
            //run hit animation
            return;
        }
        else
        {
            //run hit animation
            Player.hp -= damage;
        }
        attacking = false;
    }

    /*
     * void Die(); 
     * {
     *  Ragdoll
     *  Stop all other functions
     *  After x seconds, despawn
     * }
     */
}
