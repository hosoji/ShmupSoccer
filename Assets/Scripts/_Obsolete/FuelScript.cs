using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelScript : MonoBehaviour {


	public bool usingFuel = false;

	PlayerStats stats;

//	public float maxFuel;

	private float fuel;

//	public float regenRate;
//	public float depletionRate;
//	public float movingDepletionRate;

	public Text fuelText;

	public Image fuelUI;


	public float Fuel{
		get{
			return fuel;
		}

		set {
			fuel = value;

			if (fuel >= stats.maxFuel) {
				fuel = stats.maxFuel;
			} else if (fuel <= 0) {
				fuel = 0;
			}
		}
	}


	// Use this for initialization
	void Start () {
		stats = GetComponent<PlayerStats> ();
		fuel = GetComponent<PlayerStats> ().maxFuel;
	}
	
	// Update is called once per frame
	void Update () {
		float fuelFill = Util.remapRange (fuel, 0, stats.maxFuel, 0, 1);

		fuelUI.fillAmount = fuelFill;

		fuelText.text = Fuel.ToString ("#00.0");



		if (fuel < stats.maxFuel) {
			if (!usingFuel) {
				Fuel = fuel + stats.fuelRegenRate;
			}
		}


	}


	public void DecreaseFuel(float rate){
		if (fuel > 0) {
			Fuel = fuel - rate * Time.deltaTime;
		}
	}

//	public void DepleteFueltoMove(){
//		if (fuel > 0) {
//			Fuel = fuel - movingDepletionRate * Time.deltaTime;
//		}
//	}
		


}
