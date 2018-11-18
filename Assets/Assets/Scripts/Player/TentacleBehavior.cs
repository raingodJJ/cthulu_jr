using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleBehavior : MonoBehaviour
{
    public float maxRange;
    private float minRange;
    public float maxSpeed;
    public GameObject player;
    private Character_Direct _player;
    public TentacleState state = TentacleState.idle;
    public Vector3 target;
    private Vector3 velocity;
    public GameObject holding;
    private int grabCounter = 0;
    public int grabDuration;
    private int cooldownCounter = 0;
    public int cooldown;
    private int throwDelayCounter = 0;

    // Use this for initialization
    void Start()
    {
        _player = player.GetComponent<Character_Direct>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == TentacleState.grabbing)
        {
            if (grabCounter != grabDuration / 2)
            {
                velocity = GetDirection(target, transform.position, 0);
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
            else if (grabCounter == grabDuration / 2)
            {
                target = _player.tentaclePoint.transform.position;
                grabCounter++;
            }
        }
        else if (state == TentacleState.holding)
        {
            target = _player._aim;
            //If to far
            if (Vector3.Distance(target, _player.tentaclePoint.transform.position) > maxRange)
            {
                //Max range
                Vector3 direction = GetDirection(target, _player.tentaclePoint.transform.position, maxRange);
                target = _player.tentaclePoint.transform.position + direction;
            }
            //If to close
            else if (Vector3.Distance(target, _player.tentaclePoint.transform.position) < 2)
            {
                //Min Range 1
                Vector3 direction = GetDirection(target, _player.tentaclePoint.transform.position, 2);
                target = _player.tentaclePoint.transform.position + direction;
            }
            velocity = GetDirection(target, this.transform.position, maxSpeed);
            Vector3 pos;
            pos.x = transform.position.x + velocity.x * Time.deltaTime*1;
            pos.y = transform.position.y;
            pos.z = transform.position.z + velocity.z * Time.deltaTime*1;
            transform.position = target;
            holding.transform.position = this.transform.position;
        }
        else if (state == TentacleState.throwing)
        {
            transform.position = _player.tentaclePoint.transform.position;
            holding = null;
            state = TentacleState.cooldown;
        }
        else if (state == TentacleState.cooldown)
        {
            if (cooldownCounter < cooldown)
            {
                cooldownCounter++;
            }
            else
            {
                cooldownCounter = 0;
                state = TentacleState.idle;
            }
        }
    }

    void Fire()
    {
        if (state == TentacleState.idle)
        {
            state = TentacleState.grabbing;
            target = _player._aim;
            if (Vector3.Distance(target, _player.tentaclePoint.transform.position) > maxRange)
            {
                Vector3 direction = GetDirection(target, _player.tentaclePoint.transform.position, maxRange);
                //Vector3 direction = target - _player.tentaclePoint.transform.position;
                //direction.Normalize();
                //direction *= maxRange;
                target = _player.tentaclePoint.transform.position + direction;
            }
            else if (Vector3.Distance(target, _player.tentaclePoint.transform.position) < minRange)
            {
                Vector3 direction = target - _player.tentaclePoint.transform.position;
                direction.Normalize();
                target = _player.tentaclePoint.transform.position + direction;
            }
        }
        else if (state == TentacleState.holding)
        {
            state = TentacleState.throwing;
            target = _player._aim;
            Rigidbody rigBod = holding.GetComponent<Rigidbody>();
            rigBod.isKinematic = false;
            holding.layer = 0;
            Vector3 direction = GetDirection(target, _player.transform.position, maxSpeed);
            rigBod.velocity = direction;
            if (holding.tag == "Enemy")
            {
                ParentEnemy enemy = holding.GetComponent<ParentEnemy>();
                enemy.isGrabbed = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state == TentacleState.grabbing)
        {
            if (collision.gameObject.tag == "Moveable" || collision.gameObject.tag == "Enemy")
            {
                holding = collision.gameObject;
                Rigidbody rigBod = holding.GetComponent<Rigidbody>();
                rigBod.isKinematic = true;
                holding.layer = 9;
                state = TentacleState.holding;
            }
            if (holding.tag == "Enemy")
            {
                ParentEnemy enemy = holding.GetComponent<ParentEnemy>();
                enemy.isGrabbed = true;
            }
        }
    }

    Vector3 GetDirection(Vector3 target, Vector3 position, float Magnitude)
    {
        Vector3 direction = target - position;
        if (Magnitude == 0)
        {
            return direction;
        }
        else
        {
            direction.Normalize();
            direction *= Magnitude;
            return direction;
        }
    }
}