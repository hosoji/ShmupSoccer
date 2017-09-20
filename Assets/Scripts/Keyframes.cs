using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyframes : MonoBehaviour {

	public GameObject nodePrefab;

	Vector3 temp = new Vector3(0,0,0);

	GameObject node;
	public Transform keyFrameHolder;

	public List <Vector3> keyFrames = new List<Vector3> ();

	float yOffset = -1f;

	public void KeyFrameAssignment(Vector3 pos, bool action){


		if (pos != temp) {
			keyFrames.Add (pos);
			Vector3 keyPos = new Vector3 (pos.x, pos.y + yOffset, pos.z);
			node = Instantiate (nodePrefab, keyPos, Quaternion.identity, keyFrameHolder);
			node.transform.localEulerAngles = new Vector3 (90f, 0f, 0f);
			node.transform.localScale = new Vector3 (3.4f, 3.4f, 3.4f);
			temp = pos;
		}


	}


}
