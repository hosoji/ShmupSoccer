using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerScript : MonoBehaviour {



	public float speed = 10f;
	public float defaultSpeed; 
	public float slowDownMod = 10f;

	public float jumpSpeed = 1f;
	public float maxRotationLeft;
	public float maxRotationRight;
	[SerializeField] float defaultRotation;
	public float maxLift = 2f;
	float defaultLift = 0.1f;

	public int playerNum;


//	float snapBackMod = 6f;

	HealthScript health;
	FuelScript fuel;
	TeamAssignment input;



	public GameObject projectile;
	public float projectilePower = 0f;
	float maxProjectilePower = 10f;
	public bool projectileExist = false;
	GameObject bulletInstance;


	// Use this for initialization
	void Start () {
		health = GetComponent<HealthScript> ();
		fuel = GetComponent<FuelScript> ();
		speed = defaultSpeed;
		input = GetComponent<TeamAssignment> ();



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
//			Debug.Log ("Striker not active");
				health.vulnerable = false;
				fuel.usingFuel = false;

			}
		} else {
			if (TeamManager.p2StrikerActive) {
				PlayerControl ();
			} else {
				Debug.Log ("Striker 2 not active");
				health.vulnerable = false;
				fuel.usingFuel = false;

			}
		}
		
	}

	void PlayerControl(){



		if (!Input.anyKey){
			health.vulnerable = false;
			fuel.usingFuel = false;

			EnableSwitching (true);
		}


		float controllerVertical = Input.GetAxis (input.vertical);
		float controllerHorizontal = Input.GetAxis (input.horizontal);

		float horizontalPos = transform.position.x + controllerHorizontal * Time.deltaTime * speed;
		float verticalPos = transform.position.z + controllerVertical * Time.deltaTime * speed;

		if (fuel.Fuel > 0) {
			transform.position = new Vector3 (horizontalPos, transform.position.y, verticalPos);
		}

		if (controllerVertical != 0 || controllerHorizontal != 0) {
			health.vulnerable = true;
			fuel.usingFuel = true;

			EnableSwitching (false);

			fuel.DepleteFueltoMove ();
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
			fuel.DecreaseFuel ();
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
			
			fuel.usingFuel = true;

			if (!projectileExist) {
				if (fuel.Fuel > 0) {

					bulletInstance = Instantiate (projectile, transform.position + transform.forward, Quaternion.identity, transform);
					bulletInstance.GetComponent<BulletScript>().released = false;
					bulletInstance.GetComponent<BulletScript>().playerNum = playerNum;

					StartCoroutine ("ChargeProjectile", bulletInstance);
					projectileExist = true;
				}
			}

		}

		if (Input.GetButtonUp (input.fire) || fuel.Fuel == 0) {
			EnableSwitching (true);
			fuel.usingFuel = false;
			speed = defaultSpeed;
			if (bulletInstance != null && bulletInstance.GetComponent<BulletScript>().released == false) {
				Rigidbody rb = bulletInstance.GetComponent<Rigidbody> ();
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
			if (health.vulnerable) {
				health.DecreaseHealth ();
			}
		}
	}

	IEnumerator ChargeProjectile(GameObject bullet){
		while (Input.GetButton (input.fire) == true ) {
			health.vulnerable = true;
			EnableSwitching (false);
			fuel.DecreaseFuel ();
//			print ("true");
			if (projectilePower < maxProjectilePower && fuel.Fuel > 0) {

				if (!bulletInstance.GetComponent<BulletScript>().released) {
					projectilePower += Time.deltaTime * 10f;
					speed -= Time.deltaTime * slowDownMod;
//				print (projectilePower);
					bullet.transform.localScale += new Vector3 (0.01f, 0.01f, 0.01f);
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
