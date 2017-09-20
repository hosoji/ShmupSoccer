using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingUnit : MonoBehaviour {


	public float currentSpeed;
	public float defaultSpeed; 
	public float slowDownMod = 10f;

	public int bulletsMax = 2;

	public float maxRotationLeft;
	public float maxRotationRight;
	[SerializeField] float defaultRotation;

	public GameObject prefab;
	public List <GameObject> bullets = new List<GameObject> ();

	TeamAssignment input;
	public int playerNum;

	// Use this for initialization
	void Start () {
		input = GetComponent<TeamAssignment> ();
	}
	
	// Update is called once per frame
	void Update () {
		PlayerControl ();
//		if (Input.GetButtonUp("P1_R2")){
//			PlayerAction ();
//		}
	}

	void PlayerControl(){

		float controllerVertical = Input.GetAxis (input.vertical);
		float controllerHorizontal = Input.GetAxis (input.horizontal);

		float horizontalPos = transform.position.x + controllerHorizontal * Time.deltaTime * currentSpeed;
		float verticalPos = transform.position.z + controllerVertical * Time.deltaTime * currentSpeed;




		transform.position = new Vector3 (horizontalPos, transform.position.y, verticalPos);

//		Vector3 movement = new Vector3 (controllerHorizontal, 0f, controllerVertical);
//		rb.AddForce (movement * currentSpeed * Time.deltaTime * 10f, ForceMode.Impulse);

		float playerRotation = Util.remapRange (Input.GetAxis (input.rStick), -1, 1, maxRotationLeft, maxRotationRight);
		if (transform.eulerAngles.y <= maxRotationRight && transform.eulerAngles.y >= maxRotationLeft) {
			//			transform.Rotate (0f, turnAngle, 0f);
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x, playerRotation, transform.eulerAngles.z);
		} else {
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x, defaultRotation, transform.eulerAngles.z);
		}

	}

	public void PlayerAction(){

		if (bullets.Count < bulletsMax) {
		
			GameObject projectileInstance = Instantiate (prefab, transform.position + transform.forward * 2f, Quaternion.Euler (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z));
			ProjectileScript ps = projectileInstance.GetComponent<ProjectileScript> ();
			ps.initialVelocity = transform.forward * 20f;
			ps.striker = this;
			ps.pNum = playerNum;
			bullets.Add (projectileInstance);
		}
		

	}
}
