using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentEnemy : MonoBehaviour
{

    public GameObject player;

    public TentacleState state;

    public bool Aware = false;

    double distToGround;

    //test

    [Header("Set in Inspector: Enemy")]

    public float hitsRemain = 3f;

    public float movement = 5f;

    public int maxDamage = 1;

    public float meleeRange = 1f;


    public float MinDist = 1.5f;

    public float MaxDist = 0f;

    public float DetectionRange = 0f;

    public float attackRate = 1f;

    private float interval = 0f;

    private bool attacking = false;


    [Header("Set Dynamically: Enemy")]

    public float health;

    public float speed;

    public int damage;

    public bool invincible = false;

    public bool knockback = false;



    protected virtual void Awake()
    {
        health = hitsRemain;
        speed = movement;
        damage = maxDamage;
    }

    // Use this for initialization
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("player");

        distToGround = this.GetComponent<Collider>().bounds.extents.y;
    }



    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, (float)(distToGround + 0.1));
    }


    // Update is called once per frame
    void Update()
    {
        /*
         * if (hitsRemain <= 0)
         * {
         *  Die();
         * }
         */
        if (Vector3.Distance(transform.position, player.transform.position) <= DetectionRange)
        {
            Aware = true;
        }

        if (state != TentacleState.grabbing && IsGrounded() && Aware)
        {


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


    }

    void DealDamage()
    {
        if (attacking)
        {
            return;
        }
        attacking = true;
        if (Vector3.Distance(transform.position, player.transform.position) >= meleeRange)
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
