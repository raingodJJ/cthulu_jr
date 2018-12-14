using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterGuy : MonoBehaviour
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

    public float MinDist = 10f;

    public float MaxDist = 100f;

    public float DetectionRange = 15f;

    public float attackRate = 1f;

    private float interval = 0f;

    public bool isGrabbed = false;

    private bool _canTakeDamage;

    public Rigidbody projectile;

    public float bulletSpeed = 20.0f;

    public bool invincible = false;

    public bool knockback = false;

    private int invWindow = 50;

    private float nextFire = 0;


    // Use this for initialization
    void Start()
    {
        distToGround = this.GetComponent<Collider>().bounds.extents.y;
    }



    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, (float)(distToGround + 0.1));
    }


    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, player.transform.position) <= DetectionRange)
        {
            Aware = true;
        }

        if (state != TentacleState.grabbing && Aware)
        {


            transform.LookAt(player.transform);

            if (Vector3.Distance(transform.position, player.transform.position) >= MinDist)
            {

                transform.position += transform.forward * speed * Time.deltaTime;


                if ((Vector3.Distance(transform.position, player.transform.position) <= MaxDist))
                {
                    Invoke("Shoot", 1f);
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

    void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + attackRate;

            Rigidbody bullet = (Rigidbody)Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            bullet.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

            Destroy(bullet.gameObject, 2);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Moveable")
        {
            if (collision.gameObject.layer != 9)
            {
                if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1)
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

