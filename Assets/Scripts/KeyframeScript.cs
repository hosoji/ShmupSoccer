using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyframeScript : MonoBehaviour {

	[SerializeField] GameObject nodePrefab;
	[SerializeField] GameObject attkNodePrefab;
	[SerializeField] GameObject blockPrefab;

	Vector3 temp = new Vector3(0,0,0);

	GameObject key;
	GameObject area;
	GameObject node;
	GameObject block; 
	public Transform keyFrameHolder;

	GameManager gameManager;
	MainPlayer player;
	Recording record;


	public List<IntVector2> keyFrames = new List<IntVector2>();
	public List<Vector3> areaNodes = new List<Vector3>();
	public List <Vector3> actionFrames = new List<Vector3> ();
	List <GameObject> nodes = new List<GameObject>();
	 

	Dictionary <Vector3, GameObject> activeBlockers = new Dictionary <Vector3, GameObject>();

	float yOffset = -0.8f;

	int playerNum;

	void Start(){
		gameManager = GameObject.Find ("[GameManager]").GetComponent<GameManager> ();
		player = GetComponent<MainPlayer> ();
		record = GetComponent<Recording> ();
		playerNum = player.playerNum;


	}

	public void AreaNodeAssignment(Vector3 pos){
//		
		keyFrames.Add (GridPosition());



		if (!areaNodes.Contains(pos)) {
			Vector3 areaPos = new Vector3 (pos.x, pos.y + yOffset, pos.z);
			area = Instantiate (nodePrefab, areaPos, Quaternion.identity, keyFrameHolder);
			area.transform.localEulerAngles = new Vector3 (90f, 0f, 0f);
			area.transform.localScale = new Vector3 (3.4f, 3.4f, 3.4f);
			areaNodes.Add (pos);


		}
	}

	public void KeyFrameAssignment(){
		if (!keyFrames.Contains (GridPosition())) {
	
			keyFrames.Add (GridPosition());
		}
		
	}

	public void ActionFrameAssignment(Vector3 pos, bool action){

//		print (Vector3.Distance(pos, temp));

		if (!action) {
			if ((pos != temp || actionFrames.Count == 0) && Vector3.Distance(pos, temp) > 2f) {
				Debug.Log (" Added");
				actionFrames.Add (pos);
				Vector3 keyPos = new Vector3 (pos.x, pos.y + yOffset, pos.z);


				CreateBlocker (keyPos);


				temp = pos;
			}
		} else {

			if (pos != temp) {

				actionFrames.Add (pos);
				Vector3 keyPos = new Vector3 (pos.x, pos.y + yOffset, pos.z);
				node = Instantiate (attkNodePrefab, keyPos, Quaternion.identity, keyFrameHolder);
				node.transform.localEulerAngles = new Vector3 (90f, 0f, 0f);
				node.transform.localScale = new Vector3 (3.4f, 3.4f, 3.4f);
				nodes.Add (node);

				temp = pos;
			}
		}
	}

	public void ClearKeyFrames(){
		foreach (GameObject node in nodes) {
			Destroy (node);
		}

		actionFrames.Clear ();
		nodes.Clear ();
		keyFrames.Clear ();
		foreach (Vector3 key in activeBlockers.Keys) {
			Destroy(activeBlockers[key].gameObject);
		}

		activeBlockers.Clear ();
	}


	void CreateBlocker(Vector3 pos){

		if (!activeBlockers.ContainsKey (pos)) {
			block = Instantiate (blockPrefab, pos, Quaternion.identity) as GameObject;
			activeBlockers.Add (pos, block);

		}
	}

	public void RecreateBlocker(Vector3 pos){


		foreach (Vector3 key in activeBlockers.Keys) {

				if (activeBlockers [key] != null) {
					Debug.Log (" no nulls");
				} else {
					Debug.Log (" found a null");
					activeBlockers.Remove (key);
				}
		}
		
	}

	public bool KeyFrameDistanceCheck(Vector3 pos){
		bool hasSpace = true;
		foreach (Vector3 key in actionFrames) {
			float buffer = 0.02f;
			if (Vector3.Distance (key, pos) >= gameManager.gridSize - buffer) {
				hasSpace =  true;
			} else{
				hasSpace = false;
				break;
			}
		}
		return hasSpace;
	}
		

	public IntVector2 GridPosition(){
		return new IntVector2 (player.X, player.Z);
	}






}
