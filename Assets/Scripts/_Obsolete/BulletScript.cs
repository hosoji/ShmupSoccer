using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	Ray ray;
	Rigidbody rb;

	public bool released = false;

	public float dampenRate; 

	public float bulletPower;

	public int playerNum;

	StrikerScript player;

	RespawnScript respawn;

	TrailRenderer tr;

	public Material p1Mat, p2Mat;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody> ();
		player = transform.parent.gameObject.GetComponent<StrikerScript>();
		rb.isKinematic = true;
		respawn = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RespawnScript> ();

		tr = GetComponent<TrailRenderer> ();


	}

	void Start(){
		if (playerNum == 1) {
			tr.material = p1Mat;
		} else if (playerNum == 2) {
			tr.material = p2Mat;
		}
	}
	
	// Update is called once per frame
	void Update () {
//		ray = new Ray (transform.position, transform.forward);
//		Vector3 reflectRay = Vector3.Reflect (transform.position, ray);


		if (released) {
			

			StartCoroutine (DampenBullet ());
		}

		if (transform.localScale.x <= 0.01f){
			player.projectileExist = false;
			player.projectilePower = 0f;
			Destroy(gameObject);
		}

		bulletPower = transform.localScale.magnitude * rb.velocity.magnitude * player.projectilePower * 0.7f;
	

	}

	void OnCollisionEnter(Collision col){
		if (col.transform.tag == "Deflector") {
			print ("collided with pivot");
			Vector3 hitPos = col.contacts [0].point; 
			rb.velocity = Vector3.Reflect (hitPos - transform.position, col.contacts[0].normal ) * bulletPower;

		}

		if (col.transform.tag == "Wall") {
			ResetProjectileStatus ();
			Destroy (gameObject);
		}



		if (col.transform.tag == "Player") {
			PlayerStats stats = col.gameObject.GetComponent<PlayerStats> ();
	

			if (col.gameObject != player.gameObject) {

				if (!col.gameObject.GetComponent<HealthScript> ().vulnerable) {
					if (col.gameObject.GetComponent<HealthScript> ().Health - player.projectilePower * stats.projDamage > 0) {
						col.gameObject.GetComponent<HealthScript> ().Health -= player.projectilePower * stats.projDamage;
					} else {
						respawn.RespawnPlayer (col.gameObject);
						col.gameObject.SetActive (false);
					}

				} else {
					col.gameObject.GetComponent<HealthScript> ().Health -= player.projectilePower * stats.projDamage;
				}

				ResetProjectileStatus ();
				Destroy (gameObject);

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

	public void ResetProjectileStatus(){
		player.projectileExist = false;
		player.projectilePower = 0f;
	}



}
