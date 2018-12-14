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

    public float health = 3f;

    public float speed = 5f;

    public int damage = 1;

    public float meleeRange = 1f;

    public float MinDist = 1.5f;

    public float MaxDist = 0f;

    public float DetectionRange = 5f;

    public float attackRate = 1f;

    private float interval = 0f;

    private bool attacking = false;

    public bool isGrabbed = false;

    private bool _canTakeDamage;

    public bool invincible = false;

    public bool knockback = false;

    private int invWindow = 50;


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

        if (!_canTakeDamage)
        {
            if (invWindow > 0)
            {
                invWindow--;
            }
            else
            {
                invWindow = 50;
                _canTakeDamage = true;
            }
        }

        if (health == 0)
        {
            if (gameObject != null)
            {
                Destroy(this.gameObject);
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Moveable")
        {
            if (!isGrabbed)
            {
                if (_canTakeDamage)
                {
                    health--;
                    _canTakeDamage = false;
                }
            }
        }
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

