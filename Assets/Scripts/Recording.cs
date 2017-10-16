﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recording : MonoBehaviour {

	public List<Vector3> recPos = new List <Vector3> ();
	public List<Vector3> recRot = new List <Vector3> ();
	public List<bool> recButton = new List <bool> ();
	public List <bool> recDefense = new List<bool> ();

	List <int> moveDirections = new List<int> ();


	int lastPositionX = 0;
	int lastPositionY = 0;

	int firstDir, secondDir, thirdDir;


	Vector3 lastPos;
	Vector3 currentPos;

	public LineRenderer lr;
	public Material mat;

	public Vector3 nextPos;
	public Vector3 nextRot;

	public bool isPressed;
	public bool nextButton;
	 
	public int rotIndex = 0;
	public int posIndex = 0;
	public int buttonIndex = 0;

//	public float maxRecTime = 30f;
	float maxplayTime = 0f;
	public float  recTime = 0f;
	public float playTime = 0f;
	float recButtonStart = 0f;
	float recButtonEnd = 0f;
	float playButtonStart = 0f;
	float playButtonEnd = 0f;

	GameManager gameManager;
	MainPlayer player;
	TeamAssignment input;
	PlayerStats stats;
	KeyframeScript key;
	Playback play;
//
//	public Image recUI, playUI;
	public Text segmentText;

	public int maxSegments;
	public bool segmentRemoved = false;

	[SerializeField] private int segments;
	public int Segments{
		get { return segments; }
		private set
		{
			segments = value;
			if(segments <= 0)
			{
				segments = 0;
			}
			else if (segments > maxSegments)
			{
				segments = maxSegments;
			}
		}
	}

	private int moveCount = 0;
	public int MoveCount{
		get { return moveCount; }
		private set
		{
			moveCount = value;
			if(moveCount <= 0)
			{
				moveCount = 0;
			}
			else if (moveCount > 3)
			{
				moveCount = 0;
			}
		}
	}

	void Awake(){
		
		player = GetComponent<MainPlayer> ();
		input = GetComponent<TeamAssignment> ();
		key = GetComponent<KeyframeScript> ();
		stats = GetComponent<PlayerStats> ();
		play = GetComponent<Playback> ();

		gameManager = GameObject.Find ("[GameManager]").GetComponent<GameManager> ();
	

//		recUI.enabled = false;
//		playUI.enabled = false;


		// Set up Line Renderer 

		lr = player.gameObject.GetComponentInChildren<LineRenderer> ();
		lr.enabled = false;
		lr.widthMultiplier = 0.2f;
		lr.material = mat;
		lr.receiveShadows = true;
		lr.alignment = LineAlignment.View;
		lr.textureMode = LineTextureMode.Tile;

	}

	void Start(){
		player.enabled = true;

	
		Segments = maxSegments;
	}

	void FixedUpdate () {
		currentPos = transform.position;


	}

	void Update(){
//		RecordingPlayer ();
		play.Play ();

//		Debug.Log (gridPositionX.Count);


		ClearPath ();

		float recText = stats.maxRecTime - recTime;
		segmentText.text = Segments.ToString () + "/" + maxSegments.ToString ();


	}
		


//	public void RecordingPlayer(){
//
//		float recAmount = stats.maxRecTime - recTime;
//		float recFill = Util.remapRange (recAmount, stats.maxRecTime, 0, 1, 0);
////		recUI.fillAmount = recFill;
//
//
//		if (recAmount > 0) {
//
//			if (!play.isPlaying){
////
////				if (Input.GetButtonDown (input.recording)) {
////					key.KeyFrameAssignment (transform.position, false);
////				}
////
//				if (Input.GetButton (input.recording)) {
//					lr.enabled = true;
//					player.enabled = true;
////					recUI.enabled = true;
//					recPos.Add (transform.position);
//					recRot.Add (transform.eulerAngles);
//					PathDrawing ();
//					recTime += Time.deltaTime;
//
//
////					if (!Input.GetButtonUp (input.fire)) {
////						recButton.Add (false);
////					}
//
//					if (Input.GetButtonDown (input.fire)) {
//						recButtonStart = recTime;
//
//
//					}
//					if (Input.GetButtonUp (input.fire)) {
//
//						if (!isPressed) {
//							recButtonEnd = recTime;
//							recButton.Add (true);
//							key.ActionFrameAssignment (transform.position, true);
//							isPressed = true;
//						}
//
//						decimal duration = System.Math.Round ((decimal)(recButtonEnd - recButtonStart), 2);
//						print (duration);
//
//					} else {
//						recButton.Add (false);
//						isPressed = false;
//					}
//
//					// Places Blocks in Recording
//
//					if (Input.GetButtonUp (input.block)) {
//
//						if (!isPressed) {
//							key.ActionFrameAssignment (transform.position, false);
//							isPressed = true;
//						}
//
//					} else {
//						
//						isPressed = false;
//					}
//
//				
//				}
//
//				if (Input.GetButtonUp (input.recording)) {
////					maxplayTime =  maxplayTime + recTime;
//
//					//				recTime = 0;
//					player.enabled = false;
////					recUI.enabled = false;
//			}
//
//
//			
//			}
//		} else {
//
//
//			player.enabled = false; 
////			recUI.enabled = false;
//
//			if (Input.GetButtonUp (input.recording) && !play.isPlaying) {
//
////				key.KeyFrameAssignment (transform.position, false);
////				recTime = 0;
//			}
//		}
//	}
		


	public void PathDrawing(){

		lr.positionCount = recPos.Count;

		float yOffset = -1f;

		for (int i = 0; i < recPos.Count; i++) {
			Vector3 path = new Vector3 (recPos [i].x, recPos [i].y + yOffset, recPos [i].z);
			lr.SetPosition (i, path);
		}
	}

	public void PathCheck(){

		int dir = 0;
		// { 1 Up, -1 Down, -2 Left, 2 Right }
		
		if (Mathf.Abs (key.GridPosition ().x - lastPositionX) == 1 && key.GridPosition ().y - lastPositionY == 0 ) {
			

			if (key.GridPosition ().x - lastPositionX > 0) {
				Debug.Log ("Moved Right");
				dir = 2;
				MoveCount++;
					
			} else {
				Debug.Log ("Moved Left");
				dir = -2;
				MoveCount++;
			}
			lastPositionX = key.GridPosition ().x;
		}

		if (Mathf.Abs (key.GridPosition ().y - lastPositionY) == 1 && key.GridPosition ().x - lastPositionX == 0) {

			if (key.GridPosition ().y - lastPositionY > 0) {
				Debug.Log ("Moved Up");
				dir = 1;
				MoveCount++;

			} else {
				Debug.Log ("Moved Down");
				dir = -1;
				MoveCount++;
			}
			lastPositionY = key.GridPosition ().y;
		}

		if (moveCount == 1) {
			firstDir = dir;
			Debug.Log (firstDir);
		} else if (moveCount == 2) {

			if (Mathf.Abs(firstDir) - Mathf.Abs(dir) == 0) {
				MoveCount = 1;
				firstDir = dir;
			} else {
				secondDir = dir;
				Debug.Log (firstDir + " " + secondDir);
			}


		} else if (MoveCount == 3) {
			thirdDir = dir;
			Debug.Log (firstDir + " " + secondDir + " " + thirdDir);
			MoveCount = 0;
		}


	}
		


	void ClearPath(){
		if (!play.isPlaying && !Input.GetButton(input.recording)){
			if (Input.GetButtonDown (input.cancel)) {
				key.ClearKeyFrames ();
//				gridPositionX.Clear ();
//				gridPositionY.Clear ();


				recPos.Clear ();
				recButton.Clear ();
				recRot.Clear ();
				lr.enabled = false;
		

				recTime = 0;
				player.ResetPlayerPos ();
				ReplenishSegments ();

//				key.KeyFrameAssignment ();
//				gridPositionX.Add(key.GridPosition ().x);
//				gridPositionY.Add(key.GridPosition ().y);
				lastPositionX = 0;
				lastPositionY = 0;
				MoveCount = 0;

			}
		}
	}

	public void RecordAction(){
		if (Input.GetButtonDown (input.fire)) {
			if (!isPressed) {

				if (key.KeyFrameDistanceCheck (transform.position)) {
					recPos.Add (transform.position);
					recButton.Add (true);
					key.ActionFrameAssignment (transform.position, true);
					isPressed = true;
				}
			}
		}

		if (Input.GetButtonDown (input.block)) {
			if (!isPressed) {

				if (key.KeyFrameDistanceCheck (transform.position)) {
					recPos.Add (transform.position);
					recDefense.Add (true);
					key.ActionFrameAssignment (transform.position, false);
					isPressed = true;
				}
			}

		}
	}
		

	public void UseSegments(){

//		Debug.Log ("Being called");
		Segments--;
		segmentRemoved = true;

		
	}


	public void SegmentCheck(Vector3 pos){
		if (!key.areaNodes.Contains (pos)) {
			if (!segmentRemoved && Segments > 0) {

				UseSegments ();
			} 
		} else {
			segmentRemoved = true;
		}
	
	}


	public void ReplenishSegments(){
		Segments = maxSegments;
		
	}




}
