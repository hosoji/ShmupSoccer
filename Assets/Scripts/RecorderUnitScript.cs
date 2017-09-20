using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderUnitScript : MonoBehaviour {

	public Recording rec;


	public float rotationSpeed = 200f;

	public float speed = 23f;
	Rigidbody rb;

	TeamAssignment input;

	bool canMove = true;

	void Start(){
		rb = GetComponent<Rigidbody> ();
		input = GetComponent<TeamAssignment> ();
//		rec = GetComponent<Recording> ();
	}

	void Update(){
		if (canMove) {
			PlayerMovement ();
		}
		PlayerInput ();

	}

	void PlayerMovement(){


		float controllerVertical = Input.GetAxis (input.vertical);
		float controllerHorizontal = Input.GetAxis (input.horizontal);

		rb.AddForce (controllerHorizontal * Time.deltaTime * speed, 0f, controllerVertical * Time.deltaTime * speed, ForceMode.Impulse);
		transform.Rotate (0f, Input.GetAxis(input.rStick2) * Time.deltaTime * rotationSpeed, 0f);
	
	}

	void PlayerInput(){
		if (Input.GetButtonDown (input.recording)) {
			canMove = false;


		}

		if (Input.GetButtonUp (input.recording)) {
			canMove = true;
		}
	}



}



