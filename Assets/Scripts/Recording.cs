using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recording : MonoBehaviour {

	List<Vector3> recPos = new List <Vector3> ();
	List<Vector3> recRot = new List <Vector3> ();

	List<bool> jumpPress = new List <bool> ();

	bool isPressed;


	Vector3 nextPos;
	int posIndex = 0;
	float recTime =0f;
	float playTime = 0f;
	float recButtonStart = 0f;
	float recButtonEnd = 0f;
	float playButtonStart = 0f;
	float playButtonEnd = 0f;

	StrikerScript player;

	void Start(){
		player = GetComponent<StrikerScript> ();
		player.enabled = false;

	}

	void Update () {



		if (Input.GetButton ("P1_CIRCLE")) {
			player.enabled = true;
			recPos.Add (transform.position);

			if (Input.GetButtonDown ("P1_R2")) {
				recButtonStart = recTime;

			}
			if (Input.GetButtonUp ("P1_R2")) {
				recButtonEnd = recTime;

				decimal duration = System.Math.Round ((decimal)(recButtonEnd - recButtonStart), 2);
//				print (duration);

			}

			if (Input.GetButton ("P1_R2")) {
//				
//				jumpPress.Add (true);
//			} else {
//				jumpPress.Add(false);
			}

			recTime += Time.deltaTime;

		} else {
			recTime = 0;
		}

		if (posIndex < recPos.Count) {


	
			if (Input.GetButton ("P1_TRIANGLE")) {
				nextPos = recPos [posIndex];

				player.enabled = false;

				playTime += Time.deltaTime;

				if (playTime >= recButtonStart && playTime <= recButtonEnd) {
					print ("Button is pressed");
				}
			

				PlayRecordedMovement ();
				if (transform.position == nextPos) {
					recPos.Remove (recPos [posIndex]);
					posIndex++;

				}




//				for (int i = 0; i < jumpPress.Count; i++) {
//					StartCoroutine ("PlayRecordedActions", i);
//
//				}

			} 
				
		} else {
			recPos.Clear ();
			posIndex = 0;
			playTime = 0;
		}
//		print (recPos.Count);


	}

	void PlayRecordedMovement (){
		Vector3 velocity = Vector3.zero;
//		transform.position = Vector3.MoveTowards (transform.position, nextPos, Time.deltaTime * 9.5f);

//		transform.position = Vector3.Lerp (transform.position, nextPos, Time.deltaTime);

		transform.position = Vector3.SmoothDamp (transform.position, nextPos,ref velocity, 0.0095f * Time.deltaTime);
	}

	IEnumerator PlayRecordedActions(int index){

//		print (jumpPress[index]);
		yield return 0;

		
	}

}
