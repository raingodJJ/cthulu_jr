using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum TentacleState
{
    idle,
    grabbing,
    holding,
    throwing,
    cooldown
}
public enum Weapon
{
    crowbar,
    pistol,
    shotgun
}

[RequireComponent(typeof(CharacterController))]
public class Character_Direct : MonoBehaviour
{

    [Header("Input Axes")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string horizontalThumbstick = "Horizontal_Thumbstick";
    public string verticalThumbstick = "Vertical_Thumbstick";
    public string attackAxis = "Fire1";
    public string weaponAxis = "Fire2";

    [Header("Movement Properties")]
    public float speed = 10f;

    [Header("AimProperties")]
    public float angularSpeed = 5f;
    public Camera MainCamera;
    public Vector3 _aim;
    public Weapon weapon = global::Weapon.crowbar;

    [Header("Tentacle Properties")]
    public Transform tentaclePoint;
    public TentacleState tentacleState = TentacleState.idle;
    public GameObject tentacle;
    private TentacleBehavior _tentacle;


    private CharacterController _characterController;
    private Vector3 _characterVelocity = Vector3.zero;
    private Vector3 _thumbstickVector = Vector3.zero;

    private bool _canMove = true;
    private bool _canAim = true;
    private bool _canTakeDamage = true;
    private int invWindow = 100;
    public int HitPoints = 3;
    public RectTransform healthBar;

    private Plane _groundPlane;

    // Use this for initialization
    void Start () {
        _characterController = this.GetComponent<CharacterController>();
        _groundPlane = new Plane(Vector3.up, this.transform.position);
        _tentacle = tentacle.GetComponent<TentacleBehavior>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        healthBar.sizeDelta = new Vector2(HitPoints, healthBar.sizeDelta.y);

        if (HitPoints > 0)
        {
            if (_canMove)
            {
                Move();
            }
            else
            {
                _characterVelocity = Vector3.zero;
            }
            if (_canAim)
            {
                Aim();
            }
            if (!_canTakeDamage)
            {
                if (invWindow > 0)
                {
                    invWindow--;
                    if (invWindow == 50)
                    {
                        _canMove = true;
                    }
                }
                else
                {
                    invWindow = 100;
                    _canTakeDamage = true;
                }
            }
            Tentacle(tentacleState);
            Weapon(weapon);
            _characterController.Move(_characterVelocity * Time.deltaTime);
        }
	}

    private void Move()
    {
        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxis(horizontalAxis);
        direction.z = Input.GetAxis(verticalAxis);

        _characterVelocity = direction * speed;
        if (direction == Vector3.zero)
        {
            _characterVelocity = Vector3.zero;
        }
    }

    private void Aim()
    {
        Ray screenRay = MainCamera.ScreenPointToRay(Input.mousePosition);
        float intersection = 0.0f;

        // Set the raycast plane to the position of the player facing up
        _groundPlane.SetNormalAndPosition(Vector3.up, this.transform.position);

        // Perform a raycast to track the intersection distance of the ray
        if (_groundPlane.Raycast(screenRay, out intersection))
        {
            // Calculate the hit point on the plane and set the look at of the character transform
            _aim = screenRay.GetPoint(intersection);
            this.transform.LookAt(_aim);
        }
    }

    private void Weapon(Weapon currentWeapon)
    {
        if (Input.GetAxis(weaponAxis) > 0.5f)
        {
            if (currentWeapon == global::Weapon.crowbar)
            {
                //Do damage in a cone in front of player
            }
            if (currentWeapon == global::Weapon.pistol)
            {
                //Spawn one bullet in direction of aim
            }
            if (currentWeapon == global::Weapon.shotgun)
            {
                //Spawn seven bullets centering in direction of aim
            }
        }
    }

    private void Tentacle(TentacleState state)
    {
        if (state == TentacleState.idle)
        {
            if (Input.GetAxis(attackAxis) > 0.5f)
            {
                _tentacle.Invoke("Fire", 0.0f);
            }
            else
            {
                _canMove = true;
                _canAim = true;
            }
        }
        else if (state == TentacleState.grabbing)
        {
            _canMove = false;
            _canAim = false;
        }
        else if (state == TentacleState.holding)
        {
            if (Input.GetAxis(attackAxis) > 0.5f)
            {
                _canMove = true;
                _canAim = true;
            }
            else
            {
                _tentacle.Invoke("Fire", 0.0f);
            }
        }
        else if (state == TentacleState.cooldown)
        {
            _canMove = true;
            _canAim = true;
        }
        this.tentacleState = _tentacle.state;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (_canTakeDamage)
            {
                ParentEnemy enemy = collision.gameObject.GetComponent<ParentEnemy>();
                if (enemy.isGrabbed == false)
                {
                    HitPoints--;
                    if (HitPoints < 0)
                    {
                        _canMove = false;
                        _canAim = false;
                    }
                    _canMove = false;
                    Vector3 direction = transform.position - collision.transform.position;
                    transform.position = transform.position + direction;
                    _canTakeDamage = false;
                }        
            }

            
        }
        if (collision.gameObject.tag == "Bullet")
        {
            if (_canTakeDamage)
            {
                HitPoints--;
                Destroy(collision.rigidbody);
                {
                    _canMove = false;
                    _canAim = false;
                }
                _canMove = false;
                Vector3 direction = transform.position - collision.transform.position;
                transform.position = transform.position + direction;
                _canTakeDamage = false;
            }
        }
    }
}
