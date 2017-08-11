using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {

	public string p1SwitchLeft, p1SwitchRight, p2SwitchLeft, p2SwitchRight;

	public static bool p1StrikerActive, p1DefenderActive, p1PivotActive, p2StrikerActive, p2DefenderActive, p2PivotActive;

	public static bool p1CanSwitch = true;
	public static bool p2CanSwitch = true;

	public Canvas p1StrikerUI, p1DefenderUI, p1PivotUI, p2StrikerUI, p2DefenderUI, p2PivotUI;

	public List <GameObject> p1Units = new List <GameObject>();
	public List <GameObject> p2Units = new List <GameObject>();

	GameObject p1selectedUnit, p2selectedUnit;

	int p1Num, p2Num;


	void Update () {

		p1selectedUnit = p1Units [p1Num];
		p2selectedUnit = p2Units [p2Num];
		if (p1CanSwitch) {
			P1SwitchUnits ();
		}

		if (p2CanSwitch) {
			P2SwitchUnits ();
		}
			
	}

	void P1SwitchUnits(){
//		print (p1selectedUnit.name);
		if (Input.GetButtonDown (p1SwitchLeft)) {
			if (p1Num > 0) {
				p1Num--;
			} else {
				p1Num = p1Units.Count - 1;
			}
		}

		if (Input.GetButtonDown (p1SwitchRight)) {
			if (p1Num < p1Units.Count-1) {
				p1Num++;
			} else {
				p1Num = 0;
			}
		}


		if (p1selectedUnit == p1Units [0]) {
			if (p1Units[0].activeInHierarchy) {
				p1StrikerActive = true;
				p1DefenderActive = false;
				p1PivotActive = false;

				p1StrikerUI.enabled = true;
				p1DefenderUI.enabled = false;
				p1PivotUI.enabled = false;
			} else {
				p1Num++;

			}
		}

		if (p1selectedUnit == p1Units [1]) {
			if (p1Units [1].activeInHierarchy) {
				p1StrikerActive = false;
				if (!WaveGenerator.teamAGeneratorActive) {
					p1DefenderActive = true;
				}
				p1PivotActive = false;

				p1StrikerUI.enabled = false;
				p1DefenderUI.enabled = true;
				p1PivotUI.enabled = false;
			}else {
				p1Num++;
	
			}
		}

		if (p1selectedUnit == p1Units [2]) {
			if (p1Units [2].activeInHierarchy) {
				p1StrikerActive = false;
				p1DefenderActive = false;
				p1PivotActive = true;

				p1StrikerUI.enabled = false;
				p1DefenderUI.enabled = false;
				p1PivotUI.enabled = true;
			}else {
				p1Num--;

			}
		}
	}

	void P2SwitchUnits(){
		if (Input.GetButtonDown (p2SwitchLeft)) {
			if (p2Num > 0) {
				p2Num--;
			} else {
				p2Num = p2Units.Count - 1;
			}
		}

		if (Input.GetButtonDown (p2SwitchRight)) {
			if (p2Num < p2Units.Count-1) {
				p2Num++;
			} else {
				p2Num = 0;
			}
		}


		if (p2selectedUnit == p2Units [0]) {
			if (p2Units[0].activeInHierarchy) {
				p2StrikerActive = true;
				p2DefenderActive = false;
				p2PivotActive = false;

				p2StrikerUI.enabled = true;
				p2DefenderUI.enabled = false;
				p2PivotUI.enabled = false;
			} else {
				p2Num++;

			}
		}

		if (p2selectedUnit == p2Units [1]) {
			if (p2Units [1].activeInHierarchy) {
				p2StrikerActive = false;
				if (!WaveGenerator.teamBGeneratorActive) {
					p2DefenderActive = true;
				}
				p2PivotActive = false;

				p2StrikerUI.enabled = false;
				p2DefenderUI.enabled = true;
				p2PivotUI.enabled = false;
			}else {
				p2Num++;

			}
		}

		if (p2selectedUnit == p2Units [2]) {
			if (p2Units [2].activeInHierarchy) {
				p2StrikerActive = false;
				p2DefenderActive = false;
				p2PivotActive = true;

				p2StrikerUI.enabled = false;
				p2DefenderUI.enabled = false;
				p2PivotUI.enabled = true;
			}else {
				p2Num--;

			}
		}
	}
}
