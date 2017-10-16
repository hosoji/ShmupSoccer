using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCollisionScript : MonoBehaviour {
	
	Material activeMaterial;


	public int playerNum;


	void Start(){

		Debug.Log ("player number is: " + playerNum);
	}

	void Update(){
		
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			if (col.gameObject.GetComponent<DefenderScript> () != null && col.gameObject.GetComponent<DefenderScript> ().playerNum != playerNum) {
				Debug.Log ("Collided with Defender");
				col.gameObject.GetComponent<HealthScript>().DecreaseHealth(col.gameObject.GetComponent<PlayerStats>().waveDamage);
			    gameObject.SetActive (false);


			}
		}
	}
}


