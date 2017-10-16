using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchzoneScript : MonoBehaviour {

	public bool isReady =false;
	public Transform bullet;

	Image catchUI;

	void Start(){
		catchUI = GameObject.Find ("P1_CatchUI").GetComponent<Image>();
		bullet = null;
		catchUI.enabled = false;
	}

	void OnTriggerStay(Collider col){
		if (col.transform.tag == "Bullet") {
			catchUI.enabled = true;
			bullet = col.transform;
			isReady = true;
		} else {
			isReady = false;
		}
	}

	void OnTriggerExit (Collider col){
		if (col.transform.tag == "Bullet") {
			isReady = false;
			bullet = null;
			catchUI.enabled = false;
		} 
	}
}
