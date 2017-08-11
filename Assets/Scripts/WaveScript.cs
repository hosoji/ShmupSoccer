using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour {

	public Vector3 targetPos;
	Vector3 startPos;
	public float speed;

	public int dirMod;

	float offsetX = 2f;


	public float scaleMax, scaleMin;

	float dist;



	void Start () {
		
		startPos = transform.position;
	}

	void Update () {

		dist = Vector3.Distance (transform.position, targetPos);

		float sizeChange = Util.remapRange (dist, 25f,0f, scaleMax, scaleMin);

		transform.localScale = new Vector3 (sizeChange, transform.localScale.y, transform.localScale.z);

		Vector3 finalPos = new Vector3 (targetPos.x + offsetX * dirMod, targetPos.y, targetPos.z);

//		print (dist);
		transform.position = Vector3.MoveTowards (transform.position, finalPos , Time.deltaTime * speed);

		// x += (target.position - x ) * 0.1f

//		transform.position += (target.transform.position - transform.position) * 0.1f * Time.deltaTime;

	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Wall") {
			Debug.Log ("Collided");
			Destroy (gameObject);
		}
	}


}
