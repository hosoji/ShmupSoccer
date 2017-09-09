using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RespawnScript : MonoBehaviour {


	public float respawnDelay;


	public GameObject prefab;

	GameObject playerInstance;

	bool startRespawning = false;




	public void RespawnPlayer(GameObject player){

		startRespawning = true;
		Debug.Log ("Respawn begun for " + player.name);
		StartCoroutine(RespawnCoroutine (player));

	}

	IEnumerator RespawnCoroutine(GameObject player){
		GameObject ui;

		if (startRespawning) {

			ui = Instantiate (prefab, player.GetComponent<HealthScript> ().healthText.transform.position, Quaternion.identity, player.GetComponent<HealthScript> ().healthText.transform) as GameObject;
			ui.GetComponent<UI_Respawn> ().StartRespawn (player, respawnDelay);
		} else {
			ui = null;
		}



		yield return new WaitForSeconds (respawnDelay);
		Debug.Log ("Respawn");
		player.SetActive (true);

	
		ResetPlayer (player);

		if (ui != null) {
			Destroy (ui);
		}
		startRespawning = false;


	}

	public void ResetPlayer(GameObject player){

		if (player.GetComponentInChildren<BulletScript> () != null) {
			Destroy (player.GetComponentInChildren<BulletScript> ().gameObject);
		}

		player.GetComponent<FuelScript> ().Fuel = player.GetComponent<PlayerStats> ().maxFuel;
		player.GetComponent<HealthScript> ().Health = player.GetComponent<PlayerStats> ().maxHealth;

		player.transform.position = player.GetComponent<TeamAssignment> ().startPos;


		if (player.GetComponent<StrikerScript> () != null) {
			player.GetComponent<StrikerScript> ().currentSpeed = player.GetComponent<StrikerScript> ().defaultSpeed;
			player.GetComponent<StrikerScript> ().projectileExist = false;
		}


		if (player.GetComponent<DefenderScript> () != null) {

			player.transform.SetParent (null);
			player.transform.GetChild(0).gameObject.SetActive(true);


			GameObject[] markers = GameObject.FindGameObjectsWithTag ("Marker");

			if (markers.Length > 0) {
				for (int i = 0; i < markers.Length; i++) {
					Destroy (markers [i].gameObject);
				}
			}

			player.GetComponent<DefenderScript> ().hasJumped = false;
			player.GetComponent<DefenderScript> ().isJumping = false;
			player.GetComponent<Rigidbody> ().useGravity = true;
			player.GetComponent<Rigidbody> ().isKinematic = false;

		}
	}
}
