using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Plane _groundPlane;

    // Use this for initialization
    void Start () {
        _characterController = this.GetComponent<CharacterController>();
        _groundPlane = new Plane(Vector3.up, this.transform.position);
        _tentacle = tentacle.GetComponent<TentacleBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
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
        Tentacle(tentacleState);
        Weapon(weapon);
        _characterController.Move(_characterVelocity * Time.deltaTime);
	}

    private void Move()
    {
        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxis(horizontalAxis);
        direction.z = Input.GetAxis(verticalAxis);

        _characterVelocity = direction * speed;
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
                _tentacle.Invoke("Fire", 0.0f);
            }
            else
            {
                _canMove = true;
                _canAim = true;
            }
        }
        this.tentacleState = _tentacle.state;
    }
}
