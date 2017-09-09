using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectScript : MonoBehaviour {

	Transform reflectedBullet;
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, transform.forward, Color.yellow);
		
	}

//	void OnCollisonEnter(Collision col){
//		Debug.Log ("bullet hit pivot"); 
//		if (col.gameObject.tag == "Bullet") {
//			print (col.gameObject.GetComponent<Rigidbody> ().velocity);
//			Transform reflectedBullet = col.gameObject.transform;
//			reflectedBullet.position = Vector3.Reflect (col.gameObject.transform.position, transform.forward);
//		}
////		col.contacts[0].normal
//	}
		
}



