using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderScript : MonoBehaviour {


	Rigidbody rb;

	public float speed = 10f;

	public float rotationSpeed = 20f;

	public float maxJumpDistance, minJumpDistance;
	public float jumpSpeed = 10f;

	public GameObject marker;
	GameObject midPoint, endPoint;

	float midXMod, midYMod, endXMod, endYMod;
	Vector3 midPos= Vector3.zero;
	Vector3 endPos = Vector3.zero;

//	public AnimationCurve mid, end;


	public bool isJumping = false;
	public bool hasJumped = true;


	HealthScript health;
	FuelScript fuel;
	TeamAssignment input;

	public int playerNum;




	void Start () {
		health = GetComponent<HealthScript> ();
		fuel = GetComponent<FuelScript> ();

		rb = GetComponent<Rigidbody> ();
		input = GetComponent<TeamAssignment> ();


		if (input.myTeam == TeamAssignment.Team.TEAM_A) {
			playerNum = 1;
		} else {
			playerNum = 2;
		}

	}
	

	void Update () {
		if (playerNum == 1) {

			if (TeamManager.p1DefenderActive) {
				PlayerControl ();
			} else {
				//			Debug.Log ("Striker not active");
				health.vulnerable = false;
				fuel.usingFuel = false;

			}
		} else {
			if (TeamManager.p2DefenderActive) {
				PlayerControl ();
			} else {
				health.vulnerable = false;
				fuel.usingFuel = false;

			}
		}

	}

	void PlayerControl(){



		if (!Input.anyKey){
			if (!hasJumped) {
				health.vulnerable = false;
			}
			fuel.usingFuel = false;

			EnableSwitching (true);

		}
			

		float controllerVertical = Input.GetAxis (input.vertical);
		float controllerHorizontal = Input.GetAxis (input.horizontal);

		if (controllerVertical != 0 || controllerHorizontal != 0) {
			health.vulnerable = true;
		} 

		transform.Rotate (0f, controllerHorizontal * Time.deltaTime * rotationSpeed, 0f);

		if (Input.GetButton (input.move)) {

			health.vulnerable = true;


			EnableSwitching (false);
			
			if (fuel.Fuel > 0) {
				fuel.usingFuel = true;
				rb.AddForce (transform.forward * Time.deltaTime * speed, ForceMode.Force);
				fuel.DepleteFueltoMove ();

			}

		}



		if (Input.GetButton (input.jump)) {
	
			
			if (!isJumping && !hasJumped) {
				midXMod = 3f;
				midYMod = 1f;
				endXMod = 6f;
				endYMod = -1f;


				Vector3 midDir = transform.forward * midXMod + transform.up * midYMod;
				Vector3 endDir = transform.forward * endXMod + transform.up * endYMod;

				Vector3 midPos = transform.position + midDir;
				Vector3 endPos = transform.position + endDir;



				if (midPoint == null && endPoint == null) {
					midPoint = Instantiate (marker, midPos, Quaternion.identity, transform) as GameObject;
					endPoint = Instantiate (marker, endPos, Quaternion.identity, transform) as GameObject;
					isJumping = true;
				}
			}

			EnableSwitching (false);
			if (isJumping) {

				if (midPoint != null && endPoint != null) {

//					fuel.usingFuel = true;
			
					float dist = Vector3.Distance (transform.position, endPoint.transform.position);

					float midPosMod = -Input.GetAxis (input.rStick2) * Time.deltaTime;
					float endPosMod = Input.GetAxis (input.rStick2) * Time.deltaTime;



					if (dist >= maxJumpDistance && Input.GetAxis (input.rStick2) > 0) {
						midPoint.transform.position = midPoint.transform.position;
						endPoint.transform.position = endPoint.transform.position;
					} else if (dist <= minJumpDistance && Input.GetAxis (input.rStick2) < 0) {
						midPoint.transform.position = midPoint.transform.position;
						endPoint.transform.position = endPoint.transform.position;
					} else {

						midPoint.transform.position = midPoint.transform.position + transform.up * midPosMod;
						endPoint.transform.position = endPoint.transform.position + transform.forward * endPosMod;
					}
				}

			}
		}

		if (Input.GetButtonUp (input.jump)) {

			if (!hasJumped) {
				isJumping = false;
				hasJumped = true;
				rb.useGravity = false;


				if (endPoint != null) {
					endPos = new Vector3 (endPoint.transform.position.x, endPoint.transform.position.y - endYMod, endPoint.transform.position.z);
				}

				if (midPoint != null) {
					midPos = new Vector3 (midPoint.transform.position.x, midPoint.transform.position.y, midPoint.transform.position.z);
				}

				if (!Input.GetButton (input.cancel)) {
					StartCoroutine (ExecuteJump (midPos, endPos));
				} else {
					hasJumped = false;

				}
			}

				if (midPoint != null && endPoint != null) {
					Destroy (midPoint.gameObject);
					Destroy (endPoint.gameObject);
				}


			}
		}



	IEnumerator ExecuteJump(Vector3 mp, Vector3 ep){
		Vector3 dir = mp - transform.position;
		Debug.DrawRay (transform.position, dir,Color.yellow);
		fuel.usingFuel = true;

		float t = 0;

		while ( t<1){
			fuel.DecreaseFuel ();
			health.vulnerable = true;


			if (fuel.Fuel <= 0) {
				break;
			}
			
//			Debug.Log (rb.velocity);
			t += Time.deltaTime * 0.7f;
			rb.AddForce (dir *Time.deltaTime , ForceMode.Impulse);
			yield return 0;
		}
		rb.useGravity = true;
		hasJumped = false;
		fuel.usingFuel = false;


	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Wave") {
			if (health.vulnerable) {
				health.DecreaseHealth ();
			}
		}
	}

	void EnableSwitching ( bool b){
		if (playerNum ==1 ) {
			TeamManager.p1CanSwitch = b;
		} else {
			TeamManager.p2CanSwitch = b;
		}
	}

//	IEnumerator JumpToPosition(Vector3 midPos, Vector3 endPos){
//		float t = 0f;
//		float n = 0f;
//		Vector3 startPos = transform.position;
//		fuel.usingFuel = true;
//
//
//		while (t < 1f) {
//			t += Time.deltaTime * 0.7f;
//			fuel.DecreaseFuel ();
//
//			transform.position = Vector3.Lerp (startPos, midPos, mid.Evaluate (t));
//
//			yield return 0;
//		}
//
//		while (n < 1f) {
//			n += Time.deltaTime * 0.7f;
//			fuel.DecreaseFuel ();
//
//			transform.position = Vector3.Lerp (midPos, endPos, mid.Evaluate (n));
//
//
//			yield return 0 ;
//		}
//
//		rb.useGravity = true;
//		hasJumped = false;
//		fuel.usingFuel = false;
//
//	}
}
