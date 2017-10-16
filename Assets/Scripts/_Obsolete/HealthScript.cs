using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

	public bool vulnerable = false;

	private float health;

//	public float waveDamage;
//	public float projectileDamage;

	PlayerStats stats;

	RespawnScript respawn;


	public float Health{
		get{
			return health;
		}

		set {
			health = value;

			if (health >= stats.maxHealth) {
				health = stats.maxHealth;
			} else if (health <= 0) {
				health = 0;
			}
		}
	}

	public Text healthText;

	public Image healthUI;

	void Start () {
		stats = GetComponent<PlayerStats> ();
		health = GetComponent<PlayerStats> ().maxHealth;
		respawn = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RespawnScript> ();
	}
	

	void Update () {
		float healthFill = Util.remapRange (health, 0, stats.maxHealth, 0, 1);

		healthUI.fillAmount = healthFill;

		healthText.text = Health.ToString ("#00.0");

		if (health < stats.maxHealth && health > 0) {
			if (!vulnerable) {
				Health = health + stats.healthRegenRate;
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

			health = stats.maxHealth;
		}

	}


	public void DecreaseHealth(float rate){
		if (health > 0) {
			Health = health - rate * Time.deltaTime;
		} else {
			respawn.RespawnPlayer (gameObject);
		}
	}


	void OnTriggerStay (Collider col){
		if (col.gameObject.tag == "Wall" || col.gameObject.tag == "DamageArea") {
			DecreaseHealth (stats.outOfBoundsDamage);
		}
	}




}
