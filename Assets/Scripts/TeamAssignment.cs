using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamAssignment : MonoBehaviour {

	public enum Team {
		TEAM_A,
		TEAM_B,
	}

	public Team myTeam;

	public Vector3 startPos;

	// INPUT ASSIGNMENT //

	public string horizontal, vertical, jump, fire, rStick, rStick2, cancel, move;

	// Use this for initialization
	void Start () {

		startPos = transform.position;


		if (myTeam == Team.TEAM_A) {
			horizontal = "P1_HORIZONTAL";
			vertical = "P1_VERTICAL";
			jump = "P1_L2";
			fire = "P1_R2";
			rStick = "P1_RSTICK";
			rStick2 = "P1_RSTICK_2";
			move = "P1_R2";
			cancel = "P1_R3";
		} else if (myTeam == Team.TEAM_B) {
			horizontal = "P2_HORIZONTAL";
			vertical = "P2_VERTICAL";
			jump = "P2_L2";
			fire = "P2_R2";
			rStick = "P2_RSTICK";
			rStick2 = "P2_RSTICK_2";
			move = "P2_R2";
			cancel = "P2_R3";
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
