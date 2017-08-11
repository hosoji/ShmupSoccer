using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelScript : MonoBehaviour {


	public bool usingFuel = false;

	public float maxFuel;

	private float fuel;

	public float regenRate;
	public float depletionRate;
	public float movingDepletionRate;

	public Text fuelText;

	public Image fuelUI;


	public float Fuel{
		get{
			return fuel;
		}

		set {
			fuel = value;

			if (fuel >= maxFuel) {
				fuel = maxFuel;
			} else if (fuel <= 0) {
				fuel = 0;
			}
		}
	}


	// Use this for initialization
	void Start () {
		fuel = maxFuel;
	}
	
	// Update is called once per frame
	void Update () {
		float fuelFill = Util.remapRange (fuel, 0, maxFuel, 0, 1);

		fuelUI.fillAmount = fuelFill;

		fuelText.text = Fuel.ToString ("#00.0");



		if (fuel < maxFuel) {
			if (!usingFuel) {
				Fuel = fuel + regenRate;
			}
		}


	}


	public void DecreaseFuel(){
		if (fuel > 0) {
			Fuel = fuel - depletionRate * Time.deltaTime;
		}
	}

	public void DepleteFueltoMove(){
		if (fuel > 0) {
			Fuel = fuel - movingDepletionRate * Time.deltaTime;
		}
	}
		


}
