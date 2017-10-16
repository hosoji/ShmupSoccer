using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScript : MonoBehaviour {

	public AnimationCurve tweenCurve;
	public Transform target;

	// Use this for initialization
	void Start () {
		StartCoroutine (TweenCoroutine ());
	}
	
	IEnumerator TweenCoroutine (){
		float t = 0f;
		Vector3 startPos = transform.position;
		Vector3 endPos = target.position;

		while (t < 1) {
			t += Time.deltaTime * 0.5f;

			transform.position = Vector3.Lerp (startPos, endPos, tweenCurve.Evaluate (t));

			yield return 0;
		}
	}
}
