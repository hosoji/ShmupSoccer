using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {


	Rigidbody rb;

	public bool released = false;

	public float dampenRate; 

	public float bulletPower;

	public int playerNum;

	StrikerScript player;

	RespawnScript respawn;


	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody> ();
		player = transform.parent.gameObject.GetComponent<StrikerScript>();
		respawn = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RespawnScript> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (released) {

			StartCoroutine (DampenBullet ());
		}

		if (transform.localScale.x <= 0.01f){
			player.projectileExist = false;
			player.projectilePower = 0f;
			Destroy(gameObject);
		}

		bulletPower = transform.localScale.magnitude * rb.velocity.magnitude * player.projectilePower;


	}

	void OnTriggerEnter(Collider col){
		if (col.transform.tag == "Wall") {
			player.projectileExist = false;
			player.projectilePower = 0f;
			Destroy (gameObject);
		}

		if (col.transform.tag == "Player") {
	

			if (col.gameObject != player.gameObject) {

				if (!col.gameObject.GetComponent<HealthScript> ().vulnerable) {
					if (col.gameObject.GetComponent<HealthScript> ().Health - player.projectilePower * 5 > 0) {
						col.gameObject.GetComponent<HealthScript> ().Health -= player.projectilePower * 5;
					} else {
						respawn.RespawnPlayer (col.gameObject);
						col.gameObject.SetActive (false);
					}

				} else {
					col.gameObject.GetComponent<HealthScript> ().Health -= player.projectilePower * 5;
				}

				Destroy (gameObject);
				player.projectileExist = false;
				player.projectilePower = 0f;
			}
		
		}
	}

	IEnumerator DampenBullet(){
		float t = 0;
		while (t < 1 && released) {
			t += dampenRate * Time.deltaTime;
			transform.localScale = transform.localScale * 0.991f;
			yield return new WaitForSeconds (0.15f);
		}

			
	}
}
