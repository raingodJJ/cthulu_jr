using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static int hp = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(hp == 0)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            rb.AddExplosionForce(20f, new Vector3(), 20f);
        }
    }

}
