using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleBehavior : MonoBehaviour
{
    public float maxRange;
    public float maxSpeed;
    public GameObject player;
    private Character_Direct _player;
    public TentacleState state = TentacleState.idle;
    public Vector3 target;
    private Vector3 velocity;
    public GameObject holding;
    private int grabCounter = 0;
    public int grabDuration;

    // Use this for initialization
    void Start ()
    {
        _player = player.GetComponent<Character_Direct>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (state == TentacleState.grabbing)
        {
            if (grabCounter != grabDuration/2)
            {
                velocity = target - transform.position;
                Vector3 pos;
                pos.x = transform.position.x + velocity.x * Time.deltaTime * 11;
                pos.y = transform.position.y;
                pos.z = transform.position.z + velocity.z * Time.deltaTime * 11;
                transform.position = pos;
                grabCounter++;
                if (grabCounter == grabDuration)
                {
                    transform.position = _player.tentaclePoint.transform.position;
                    grabCounter = 0;
                    state = TentacleState.idle;
                }
            }
            else if (grabCounter == grabDuration/2)
            {
                target = _player.tentaclePoint.transform.position;
                grabCounter++;
            }
        }
        else if (state == TentacleState.holding)
        {
            holding.transform.position = this.transform.position;
            velocity = _player._aim - transform.position;
            Vector3 pos;
            pos.x = transform.position.x + velocity.x * Time.deltaTime * 11;
            pos.y = transform.position.y;
            pos.z = transform.position.z + velocity.z * Time.deltaTime * 11;
            transform.position = pos;
        }
        else if (state == TentacleState.throwing)
        {
            holding = null;
            state = TentacleState.idle;
        }
	}

    void Fire()
    {
        if (state == TentacleState.idle)
        {
            state = TentacleState.grabbing;
            target = _player._aim;
            //target too far
        }
        else if (state == TentacleState.holding)
        {
            state = TentacleState.throwing;
            target = _player._aim;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state == TentacleState.grabbing)
        {
            if (collision.gameObject.tag == "Moveable")
            {
                holding = collision.gameObject;
                Rigidbody rigBod = holding.GetComponent<Rigidbody>();
                rigBod.isKinematic = true;
                state = TentacleState.holding;
            }
        }
    }
}
