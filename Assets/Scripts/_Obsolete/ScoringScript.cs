using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringScript : MonoBehaviour {

	float points;

	public enum Team {
		TEAM_A_GOAL,
		TEAM_B_GOAL,
	}

	public Team myTeam;

	void OnCollisionEnter (Collision col){
		if (col.gameObject.tag == "Bullet") {

			points = 0.1f;
			ScreenShakeScript.shakeStrength = points * 0.2f;

			if (myTeam == Team.TEAM_A_GOAL) {
				if (col.gameObject.GetComponent<ProjectileScript> ().pNum == 1) {
					ScoreManager.p1Score -= points;
					col.gameObject.GetComponent<ProjectileScript> ().player.bullets.Remove(col.gameObject);
					DestroyBullet (col.gameObject);
				} else {
					ScoreManager.p2Score += points;
					col.gameObject.GetComponent<ProjectileScript> ().player.bullets.Remove(col.gameObject);
					DestroyBullet (col.gameObject);
				}
			} else {
				if (col.gameObject.GetComponent<ProjectileScript> ().pNum == 2) {
					ScoreManager.p2Score -= points; 
					col.gameObject.GetComponent<ProjectileScript> ().player.bullets.Remove(col.gameObject);
					DestroyBullet (col.gameObject);
				} else {
					ScoreManager.p1Score += points; 
					col.gameObject.GetComponent<ProjectileScript> ().player.bullets.Remove(col.gameObject);
					DestroyBullet (col.gameObject);
				}
			}
		}
	}

	void DestroyBullet(GameObject bullet){
		Destroy (bullet);
	}
}

