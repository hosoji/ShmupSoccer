using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

	public bool vulnerable = false;

	public float maxHealth;

	private float health;

	public float waveDamage;
	public float regenRate;

	RespawnScript respawn;


	public float Health{
		get{
			return health;
		}

		set {
			health = value;

			if (health >= maxHealth) {
				health = maxHealth;
			} else if (health <= 0) {
				health = 0;
			}
		}
	}

	public Text healthText;

	public Image healthUI;

	void Start () {
		health = maxHealth;
		respawn = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RespawnScript> ();
	}
	

	void Update () {
		float healthFill = Util.remapRange (health, 0, maxHealth, 0, 1);

		healthUI.fillAmount = healthFill;

		healthText.text = Health.ToString ("#00.0");

		if (health < maxHealth) {
			if (!vulnerable) {
				Health = health + regenRate;
			}
		}

		if (health <= 0) {

			respawn.RespawnPlayer (gameObject);
			if (gameObject.GetComponent<TeamAssignment> ().myTeam == TeamAssignment.Team.TEAM_A) {

				gameObject.SetActive (false);
				TeamManager.p1CanSwitch = true;
			} else {
				gameObject.SetActive (false);
				TeamManager.p2CanSwitch = true;
			}

			health = maxHealth;
		}

	}


	public void DecreaseHealth(){
		if (health > 0) {
			Health = health - waveDamage * Time.deltaTime;
		}
	}


	void OnTriggerStay (Collider col){
		if (col.gameObject.tag == "Wall") {
			Health = health - waveDamage * Time.deltaTime;
		}
	}


}
