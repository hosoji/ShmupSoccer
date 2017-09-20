using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recording : MonoBehaviour {

	public List<Vector3> recPos = new List <Vector3> ();
	List<Vector3> recRot = new List <Vector3> ();
	List<bool> recButton = new List <bool> ();

	Keyframes key;
	Vector3 lastPos;
	Vector3 currentPos;

	LineRenderer lr;
	public Material mat;

	Vector3 nextPos;
	Vector3 nextRot;

	bool isPressed;
	bool nextButton;
	bool isPlaying = false;
	 
	int rotIndex = 0;
	int posIndex = 0;
	int buttonIndex = 0;

	public float maxRecTime = 30f;
	float maxplayTime = 0f;
	float recTime =0f;
	float playTime = 0f;
	float recButtonStart = 0f;
	float recButtonEnd = 0f;
	float playButtonStart = 0f;
	float playButtonEnd = 0f;

	float t = 0;
	float r = 0;
	float b = 0;

	public float playbackSpeed = 0.02f;

	AttackingUnit player;
	TeamAssignment input;

	public Image rec, play;

	void Start(){


		player = GetComponent<AttackingUnit> ();
		input = GetComponent<TeamAssignment> ();
		key = GetComponent<Keyframes> ();
		player.enabled = false;


		rec.enabled = false;
		play.enabled = false;


		// Set up Line Renderer 

		lr = player.gameObject.GetComponentInChildren<LineRenderer> ();
		lr.enabled = false;
		lr.widthMultiplier = 0.2f;
		lr.material = mat;
		lr.receiveShadows = true;
		lr.alignment = LineAlignment.View;
		lr.textureMode = LineTextureMode.Tile;

	}

	void FixedUpdate () {
		currentPos = transform.position;

		RecordingPlayer ();
		Playback ();
//		Rewind ();

	}

//	// Using this for keyframes, remove if not required
//	void LateUpdate(){
//		
//		if (currentPos != lastPos) {
//			Vector3 dir = currentPos - lastPos;
//		}
//
//		lastPos = currentPos;
//	}



	public void RecordingPlayer(){

		float recAmount = maxRecTime - recTime;
		float recFill = Util.remapRange (recAmount, maxRecTime, 0, 1, 0);
		rec.fillAmount = recFill;


		if (recAmount > 0) {

			if (!isPlaying){

				if (Input.GetButtonDown (input.recording)) {
					key.KeyFrameAssignment (transform.position, false);
				}

				if (Input.GetButton (input.recording)) {
					lr.enabled = true;
					player.enabled = true;
					rec.enabled = true;
					recPos.Add (transform.position);
					recRot.Add (transform.eulerAngles);
					PathDrawing ();


					if (!Input.GetButtonUp (input.fire)) {
						recButton.Add (false);
					}

					if (Input.GetButtonDown (input.fire)) {
						recButtonStart = recTime;

					}
					if (Input.GetButtonUp (input.fire)) {
						recButtonEnd = recTime;
						recButton.Add (true);

						decimal duration = System.Math.Round ((decimal)(recButtonEnd - recButtonStart), 2);
						print (duration);
					}

					recTime += Time.deltaTime;

				}
			}

			if (Input.GetButtonUp (input.recording)) {
				maxplayTime =  maxplayTime + recTime;

				key.KeyFrameAssignment (transform.position, false);

				recTime = 0;
				player.enabled = false;
				rec.enabled = false;
			
			}
		} else {


			player.enabled = false; 
			rec.enabled = false;

			if (Input.GetButtonUp (input.recording)) {
				maxplayTime = maxplayTime + recTime;
				key.KeyFrameAssignment (transform.position, false);
				recTime = 0;
			}
		}
	}



	public void Playback(){

//		Debug.Log ("Max playtime: " + maxplayTime);

		float playAmount = maxplayTime - playTime;
		float playFill = Util.remapRange (playAmount, maxplayTime, 0, 1, 0);

		play.fillAmount = playFill;


		// Replaying the Player Movement 
		if (posIndex < recPos.Count) {

			if (!Input.GetButton (input.recording)) {

				if (Input.GetButtonDown (input.play)){
					if (!isPlaying) {
						isPlaying = true;
					} else {
						isPlaying = false;
					}
				}



				if (isPlaying) {

					nextPos = recPos [posIndex];
					play.enabled = true;
					rec.enabled = false;

					playTime += Time.deltaTime;

//					if (playTime >= recButtonStart && playTime <= recButtonEnd) {
//						print ("Button is pressed");
//					}
					transform.position = nextPos;

					if (transform.position == nextPos) {

//						recPos.Remove (recPos [posIndex]);
						posIndex++;

						// Used to slow down incrementing
//						t += Time.deltaTime;
//						if (t >= playbackSpeed) { 
//							posIndex++;
//							t = 0;
//							Debug.Log (posIndex);
//						}
					}
				}
			}

		} else {
//			recPos.Clear ();
			posIndex = 0;
			playTime = 0;
			play.enabled = false;
			isPlaying = false;

		}

		// Replaying the Player Rotation 

		if (rotIndex < recRot.Count) {

			if (isPlaying) {
				nextRot = recRot [rotIndex];
				transform.eulerAngles = nextRot;
				if (transform.eulerAngles == nextRot) {
//					recRot.Remove (recRot [rotIndex]);

					// Used to slow down incrementing
					r += Time.deltaTime;
					if (r >= playbackSpeed) { 
						rotIndex++;
						r = 0;
					}
				}	
			} 
	
		} else {
//			recRot.Clear ();
			rotIndex = 0;
		}


		// Replaying Player Actions

		if (buttonIndex < recButton.Count) {

			if (isPlaying) {
				nextButton = recButton [buttonIndex];
				bool action = nextButton;
				if (action == true) {
					player.PlayerAction ();
				}
				if (action == nextButton) {
					
//					recButton.Remove (recButton [buttonIndex]);

					// Used to slow down incrementing
					b += Time.deltaTime;
					if (b >= playbackSpeed) { 
						buttonIndex++;
						b = 0;
					}
				}	
			} 

		} else {
//			recButton.Clear ();
			buttonIndex = 0;
		}
	}
//
//	void Rewind(){
//
//		if (!Input.GetButton (input.recording)) {
//
//			if (Input.GetButton (input.rewind)) {
//				Debug.Log ("Method called");
//				posIndex = recPos.Count-1;
//				isPlaying = false;
//
//				nextPos = recPos [posIndex];
//
//				t += Time.deltaTime;
//				if (t >= playbackSpeed * 3f) { 
//					posIndex--;
//					t = 0;
//
//					print (t);
//				}
//
//			}
//		}
//
//	}

	void PathDrawing(){

		lr.positionCount = recPos.Count;

		float yOffset = -1f;

		for (int i = 0; i < recPos.Count; i++) {
			Vector3 path = new Vector3 (recPos [i].x, recPos [i].y + yOffset, recPos [i].z);
			lr.SetPosition (i, path);
		}
	}


		


}
