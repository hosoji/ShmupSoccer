using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotScriptv2 : MonoBehaviour {

	public Collider edge1, edge2;
	public GameObject section1, section2;
	public CatchzoneScript zone1, zone2;

	[SerializeField] bool isReflecting = false;

	Rigidbody rb;

	Collider mainCollider;

	Transform bullet;
	Rigidbody bulletRB;

	float velo = 0;
	float newVelo = 0;
	float multiplier = 0;
	public float multiplierMax;

	HealthScript health;
	FuelScript fuel;
	TeamAssignment input;
	PlayerStats stats;

	public float speed = 10f;

	public int playerNum;

	// Use this for initialization
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

		mainCollider = GetComponent<BoxCollider> ();

		zone1.gameObject.SetActive(false);
		zone2.gameObject.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (playerNum == 1) {

			if (TeamManager.p1PivotActive) {
				PlayerControl ();
			} else {

				health.vulnerable = false;
				fuel.usingFuel = false;

			}
		} else {
			if (TeamManager.p2PivotActive) {
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
				if (!Input.GetButtonDown (button)) {
					health.vulnerable = false;
					fuel.usingFuel = false;


					EnableSwitching (true);
				}

			}
		} else {
			foreach (string button in input.p2Inputs) {
				if (!Input.GetButtonDown (button)) {
					health.vulnerable = false;
					fuel.usingFuel = false;


					EnableSwitching (true);
				}

			}
		}

		float rstickVertical = Input.GetAxis (input.rStick);
		float rstickHorizontal = Input.GetAxis (input.rStick2);

		if (input.myTeam == TeamAssignment.Team.TEAM_A) {

			foreach (string button in input.p1Inputs) {
				if (!Input.GetButtonDown (button)) {
					health.vulnerable = false;
					fuel.usingFuel = false;

					rb.isKinematic = false;
					EnableSwitching (true);

					isReflecting = false;


					ActivatePivotParts (true, true, true, true, false, false);

				}

			}
		}

		float controllerVertical = Input.GetAxis (input.vertical);
		float controllerHorizontal = Input.GetAxis (input.horizontal);

		if (fuel.Fuel > 0) {
			rb.AddForce (controllerHorizontal * Time.deltaTime * speed, 0f, controllerVertical * Time.deltaTime * speed, ForceMode.Impulse);
		}


		if (controllerVertical != 0 ) {
			EnableSwitching (false);
			health.vulnerable = true;
			fuel.usingFuel = true;
			fuel.DecreaseFuel (stats.moveFuelDepleteRate);
		} 

		if (rstickVertical < 0) {
			ActivatePivotParts (true, false, true, false, false, true);


			rb.isKinematic = true;
			isReflecting = true;



		} else if (rstickVertical > 0) {
			ActivatePivotParts (false, true, false, true, true, false);

			rb.isKinematic = true;
			isReflecting = true;


		}

//		if (!Mathf.Approximately(Input.GetAxis(input.rStick2),0f)){
//			health.vulnerable = true;
//		}

		transform.Rotate (0f, Input.GetAxis(input.rStick2) * Time.deltaTime * 200f, 0f);

		if (Input.GetButtonDown(input.fire)) {
			EnableSwitching (false);

			rb.isKinematic = true;

			if (GetComponentInChildren<CatchzoneScript> ().enabled && GetComponentInChildren<CatchzoneScript> ().isReady) {
				bullet = GetComponentInChildren<CatchzoneScript> ().bullet;

				bullet.SetParent (transform);
				bullet.transform.position = GetComponentInChildren<CatchzoneScript> ().transform.position;

				bulletRB = bullet.gameObject.GetComponent<Rigidbody> ();
				velo = bulletRB.velocity.magnitude;

				print ("Bullet Velocity: " + velo);
				bulletRB.isKinematic = true;

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

	}

	void ActivatePivotParts(bool s1, bool s2, bool e1, bool e2, bool z1, bool z2){
		section1.SetActive (s1);
		section2.SetActive (s2);

		edge1.enabled = e1;
		edge2.enabled = e2;

		zone1.gameObject.SetActive(z1);
		zone2.gameObject.SetActive(z2);

	}

	void EnableSwitching ( bool b){
		if (playerNum ==1 ) {
			TeamManager.p1CanSwitch = b;
		} else {
			TeamManager.p2CanSwitch = b;
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
}
