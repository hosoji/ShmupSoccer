using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerScript : MonoBehaviour {



	public float currentSpeed;
	public float defaultSpeed; 
	public float slowDownMod = 10f;

	public float jumpSpeed = 1f;

	public float maxRotationLeft;
	public float maxRotationRight;
	[SerializeField] float defaultRotation;
	public float maxLift = 2f;
	float defaultLift = 0.1f;

	public int playerNum;

//	Rigidbody rb;
//



//	float snapBackMod = 6f;

	HealthScript health;
	FuelScript fuel;
	TeamAssignment input;
	PlayerStats stats;



	public GameObject projectile;
	public float projectilePower = 0f;
	float maxProjectilePower = 10f;
	public bool projectileExist = false;
	GameObject bulletInstance;


	// Use this for initialization
	void Start () {
		stats = GetComponent<PlayerStats> ();
		health = GetComponent<HealthScript> ();
		fuel = GetComponent<FuelScript> ();
		currentSpeed = defaultSpeed;
		input = GetComponent<TeamAssignment> ();
//		rb = GetComponent<Rigidbody> ();



		if (input.myTeam == TeamAssignment.Team.TEAM_A) {
			playerNum = 1;
		} else {
			playerNum = 2;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (playerNum == 1) {

			if (TeamManager.p1StrikerActive) {
				PlayerControl ();
			} else {

				health.vulnerable = false;
				fuel.usingFuel = false;

			}
		} else {
			if (TeamManager.p2StrikerActive) {
				PlayerControl ();
			} else {
				health.vulnerable = false;
				fuel.usingFuel = false;

			}
		}

	}

	void PlayerControl(){


		if (input.myTeam == TeamAssignment.Team.TEAM_A) {

			foreach (string button in input.p1Inputs) {
				if (!Input.GetButton (button)) {
					health.vulnerable = false;
					fuel.usingFuel = false;
//					rb.isKinematic = true;


					EnableSwitching (true);
				} else {
					health.vulnerable = true;
					fuel.usingFuel = true;
//					rb.isKinematic = false;
//

					EnableSwitching (false);
					break;
				}

			}


		} else if (input.myTeam == TeamAssignment.Team.TEAM_B)  {
			foreach (string button in input.p2Inputs) {
				if (!Input.GetButton (button)) {
					health.vulnerable = false;
					fuel.usingFuel = false;
//					rb.isKinematic = true;


					EnableSwitching (true);
				} else {
					health.vulnerable = true;
					fuel.usingFuel = true;
//					rb.isKinematic = false;


					EnableSwitching (false);
					break;
				}

			}
		}


		float controllerVertical = Input.GetAxis (input.vertical);
		float controllerHorizontal = Input.GetAxis (input.horizontal);

		float horizontalPos = transform.position.x + controllerHorizontal * Time.deltaTime * currentSpeed;
		float verticalPos = transform.position.z + controllerVertical * Time.deltaTime * currentSpeed;



		if (fuel.Fuel > 0) {
			transform.position = new Vector3 (horizontalPos, transform.position.y, verticalPos);

			Vector3 movement = new Vector3 (controllerHorizontal, 0f, controllerVertical);

//			rb.AddForce (movement * currentSpeed * Time.deltaTime * 10f, ForceMode.Impulse);
		}

		if (controllerVertical != 0 || controllerHorizontal != 0) {
			health.vulnerable = true;
			fuel.usingFuel = true;

			EnableSwitching (false);
//			rb.isKinematic = false;

			fuel.DecreaseFuel(stats.moveFuelDepleteRate);
		} 

//
//		if (Input.GetButton ("PS4_R1")) {
//			playerMoving = true;
//			float angle = 1f * Time.deltaTime * rotationSpeed;
//			if (transform.eulerAngles.y <= maxRotationRight) {
//				transform.Rotate (0f, angle, 0f);
//			}
//		} else {
//			float angle = -1f * Time.deltaTime * rotationSpeed * snapBackMod;
//			if (transform.eulerAngles.y > defaultRotation) {
//
//				transform.Rotate (0f, angle, 0f);
//			} 
//		}



		if (Input.GetButton (input.jump)) {

			health.vulnerable = true;
			fuel.usingFuel = true;
			fuel.DecreaseFuel (stats.actionFuelDepleteRate);
			EnableSwitching (false);

			float elevation = transform.position.y + 1f * Time.deltaTime * jumpSpeed;

			if (fuel.Fuel > 0) {

				if (elevation < maxLift) {
					transform.position = new Vector3 (horizontalPos, elevation, verticalPos);
				} else {
					transform.position = new Vector3 (horizontalPos, maxLift, verticalPos);
				}
			} else {
				 elevation = transform.position.y + -1f * Time.deltaTime * jumpSpeed * 2f;

				if (elevation > defaultLift) {
					transform.position = new Vector3 (horizontalPos, elevation, verticalPos);
				} else {
					transform.position = new Vector3 (horizontalPos, defaultLift, verticalPos);
				}
				
			}
		} else {

			float elevation = transform.position.y + -1f * Time.deltaTime * jumpSpeed * 2f;

			if (elevation > defaultLift) {
				transform.position = new Vector3 (horizontalPos, elevation, verticalPos);
			} else {
				transform.position = new Vector3 (horizontalPos, defaultLift, verticalPos);
//				fuel.usingFuel = false;
			}
		}

		if (Input.GetButtonUp (input.jump)) {
			fuel.usingFuel = false;
			EnableSwitching (true);

		}

		if (Input.GetButtonDown(input.fire)){


			if (!projectileExist) {
				if (fuel.Fuel > 0) {
					

					bulletInstance = Instantiate (projectile, transform.position + transform.forward * 2f, Quaternion.identity, transform);
					bulletInstance.GetComponent<BulletScript>().released = false;
					bulletInstance.GetComponent<BulletScript>().playerNum = playerNum;
					bulletInstance.GetComponent<Collider> ().enabled = false;

					StartCoroutine ("ChargeProjectile", bulletInstance);
					projectileExist = true;
				}
			}

		}

		if (Input.GetButtonUp (input.fire)) {
			EnableSwitching (true);
//			fuel.usingFuel = false;
			currentSpeed = defaultSpeed;
			if (bulletInstance != null && bulletInstance.GetComponent<BulletScript>().released == false) {
				bulletInstance.GetComponent<Collider> ().enabled = true;
				Rigidbody rb = bulletInstance.GetComponent<Rigidbody> ();
				rb.isKinematic = false;
				rb.AddForce (transform.forward * projectilePower * Time.deltaTime * 100f, ForceMode.Impulse);
				bulletInstance.transform.parent = null;


				bulletInstance.GetComponent<BulletScript>().released = true;

			}
		}

//		float turnAngle = Input.GetAxis ("PS4_RSTICK") * Time.deltaTime * rotationSpeed;

//		if (!Mathf.Approximately(Input.GetAxis("PS4_RSTICK"),0f)){
//			health.vulnerable = true;
//		}

		if (playerNum == 1) {
			float playerRotation = Util.remapRange (Input.GetAxis (input.rStick), -1, 1, maxRotationLeft, maxRotationRight);
			if (transform.eulerAngles.y <= maxRotationRight && transform.eulerAngles.y >= maxRotationLeft) {
//			transform.Rotate (0f, turnAngle, 0f);
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, playerRotation, transform.eulerAngles.z);
			} else {
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, defaultRotation, transform.eulerAngles.z);
			}
		} else {



			float playerRotation = Util.remapRange (Input.GetAxis (input.rStick), 1, -1, maxRotationLeft, maxRotationRight);
			if (transform.eulerAngles.y <= maxRotationRight && transform.eulerAngles.y >= maxRotationLeft) {
//				Debug.Log ("Within angle");
				//			transform.Rotate (0f, turnAngle, 0f);
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, playerRotation, transform.eulerAngles.z);
			} else {
//				Debug.Log ("Not within angle");
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, defaultRotation, transform.eulerAngles.z);
			}
		}

		if (gameObject.activeSelf == false) {
//			Debug.Log ("Inactive");
			Destroy (bulletInstance);
		}


			
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Wave") {
			if (playerNum != col.gameObject.GetComponent<WaveCollisionScript> ().playerNum) {
				if (health.vulnerable) {
					health.DecreaseHealth (stats.waveDamage);
				}
			}
		}
	}

	IEnumerator ChargeProjectile(GameObject bullet){

		fuel.usingFuel = true;
		while (Input.GetButton (input.fire) == true ) {

			bullet.GetComponent<Collider> ().enabled = false;
			
			health.vulnerable = true;
			EnableSwitching (false);

			fuel.DecreaseFuel (stats.actionFuelDepleteRate);
//			print ("true");
			if (projectilePower < maxProjectilePower && fuel.Fuel > 0) {

				if (!bulletInstance.GetComponent<BulletScript>().released) {
					projectilePower += Time.deltaTime * 10f;
					currentSpeed -= Time.deltaTime * slowDownMod;
//				print (projectilePower);
//					bullet.transform.localScale += new Vector3 (0.01f, 0.01f, 0.01f);
				}


				yield return 0;
			}
			yield return 0;
		}
	}

	void EnableSwitching ( bool b){
		if (playerNum ==1 ) {
			TeamManager.p1CanSwitch = b;
		} else {
			TeamManager.p2CanSwitch = b;
		}
	}
}
