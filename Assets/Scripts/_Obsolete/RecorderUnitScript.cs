using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecorderUnitScript : MonoBehaviour {

	GameManager gameManager;

	public Recording rec;
	public Playback play;

	public Text healthText;
	public float health = 99.9f;

	public float rotationSpeed = 200f;

	public float speed = 23f;
	Rigidbody rb;

	TeamAssignment input;

	bool canMove = true;

	void Start(){
		rb = GetComponent<Rigidbody> ();
		input = GetComponent<TeamAssignment> ();
	
		gameManager = GameObject.Find ("[GameManager]").GetComponent<GameManager> ();
//		rec = GetComponent<Recording> ();

	}

	void Update(){
		if (canMove) {
//			PlayerMovement ();
			MovePlayer ();
		}
		PlayerInput ();

		healthText.text = health.ToString ("#00.0");

	}

	void PlayerMovement(){


		float controllerVertical = Input.GetAxis (input.vertical);
//		float controllerHorizontal = Input.GetAxis (input.horizontal);

		rb.AddForce (0f, 0f, controllerVertical * Time.deltaTime * speed, ForceMode.Impulse);
//		transform.Rotate (0f, Input.GetAxis(input.rStick2) * Time.deltaTime * rotationSpeed, 0f);
	
	}

	void PlayerInput(){
		if (Input.GetButtonDown (input.recording)) {
			canMove = false;


		}

		if (Input.GetButtonUp (input.recording)) {
			canMove = true;
		}
	}

	void MovePlayer(){
		if (play.isPlaying) {
			Vector3 pos = new Vector3 (transform.position.x, transform.position.y, rec.gameObject.transform.position.z);
			transform.position = pos;
		}
			
	}



}



