using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchzoneScript : MonoBehaviour {

	public bool isReady =false;
	public Transform bullet;

	public Image catchUI;

	void Start(){
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
