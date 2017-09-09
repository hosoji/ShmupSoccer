using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotScript : MonoBehaviour {


	public float speed = 10f;
	float defaultSpeed;

	float defaultRotation = 90f;


	Rigidbody rb;

	Transform bullet;
	Rigidbody bulletRB;

	float velo = 0;
	float newVelo = 0;
	float multiplier = 0;
	public float multiplierMax;


	public int playerNum;


	HealthScript health;
	FuelScript fuel;
	TeamAssignment input;
	PlayerStats stats;



	void Start () {
		health = GetComponent<HealthScript> ();
		fuel = GetComponent<FuelScript> ();
		rb = GetComponent<Rigidbody> ();
		input = GetComponent<TeamAssignment> ();
		stats = GetComponent<PlayerStats> ();



		bullet = null;

		if (input.myTeam == TeamAssignment.Team.TEAM_A) {
			playerNum = 1;
		} else {
			playerNum = 2;
		}
	}
	

	void Update () {

		if (playerNum == 1) {

			if (TeamManager.p1PivotActive) {
				PlayerControl ();
			} else {
	
				health.vulnerable = false;
				fuel.usingFuel = false;
				rb.isKinematic = true;

			}
		} else {
			if (TeamManager.p2PivotActive) {
				PlayerControl ();
			} else {

				health.vulnerable = false;
				fuel.usingFuel = false;
				rb.isKinematic = true;

			}
		}
		
	}

	void PlayerControl(){


		if (input.myTeam == TeamAssignment.Team.TEAM_A) {

			foreach (string button in input.p1Inputs) {
				if (!Input.GetButtonDown (button)) {
					health.vulnerable = false;
					fuel.usingFuel = false;

					rb.isKinematic = false;
					EnableSwitching (true);
				}

			}
		} else {
			foreach (string button in input.p2Inputs) {
				if (!Input.GetButtonDown (button)) {
					health.vulnerable = false;
					fuel.usingFuel = false;

					rb.isKinematic = false;
					EnableSwitching (true);
				}

			}
		}



		float controllerVertical = Input.GetAxis (input.vertical);
		float controllerHorizontal = Input.GetAxis (input.horizontal);

		if (fuel.Fuel > 0) {
			rb.AddForce (controllerHorizontal * Time.deltaTime * speed, 0f, controllerVertical * Time.deltaTime * speed, ForceMode.Impulse);
		}


		if (controllerVertical != 0 || controllerHorizontal != 0) {
			EnableSwitching (false);
			health.vulnerable = true;
			fuel.usingFuel = true;
			fuel.DecreaseFuel (stats.moveFuelDepleteRate);
		} 


		if (Input.GetButtonDown(input.fire)) {
			EnableSwitching (false);

		
		
			rb.isKinematic = true;

			if (GetComponentInChildren<CatchzoneScript> ().isReady) {
				bullet = GetComponentInChildren<CatchzoneScript> ().bullet;

				bullet.SetParent (transform);
				bullet.transform.position = GetComponentInChildren<CatchzoneScript> ().transform.position;

				bulletRB = bullet.gameObject.GetComponent<Rigidbody> ();
				velo = bulletRB.velocity.magnitude;

				print ("Bullet Velocity: " + velo);
				bulletRB.isKinematic = true;

				bullet.transform.localScale *= 2;

				bullet.GetComponent<BulletScript>().released = false;
				StartCoroutine ("ChargeProjectile", bullet.gameObject);

			}
		}



		if (Input.GetButtonUp(input.fire) ||fuel.Fuel == 0 ) {
			
			EnableSwitching (true);

			rb.isKinematic = false;
			newVelo = velo * multiplier;

			if (bullet != null && bullet.GetComponent<BulletScript>().released == false) {
				
				ReleaseProjectile (newVelo);
				multiplier = 0f;
			}	
		}




		if (!Mathf.Approximately(Input.GetAxis(input.rStick2),0f)){
			health.vulnerable = true;
		}

		transform.Rotate (0f, Input.GetAxis(input.rStick2) * Time.deltaTime * 200f, 0f);


			
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
		while (Input.GetButton (input.fire) == true ) {

			fuel.usingFuel = true;
			health.vulnerable = true;
			fuel.DecreaseFuel (stats.actionFuelDepleteRate);

			if (multiplier < multiplierMax && fuel.Fuel > 0) {

				if (!bullet.GetComponent<BulletScript>().released) {
					multiplier += Time.deltaTime * 10f;

					bullet.transform.localScale += new Vector3 (0.005f, 0.005f, 0.005f);
				}

				yield return 0;
			}
			yield return 0;
		}
	}

	void ReleaseProjectile (float speed){

		bulletRB.isKinematic = false;
		bulletRB.AddForce (transform.forward * speed * Time.deltaTime * 10f, ForceMode.Impulse);
		bullet.transform.parent = null;
		bullet.GetComponent<BulletScript>().released = true;
		
	}

	void EnableSwitching ( bool b){
		if (playerNum ==1 ) {
			TeamManager.p1CanSwitch = b;
		} else {
			TeamManager.p2CanSwitch = b;
		}
	}
}
