using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {

	public GameObject prefab;

	public Transform source1, source2;

	public float healthDepletion;



//	public static int teamNum;
	public static bool teamAGeneratorActive = false;
	public static bool teamBGeneratorActive = false;

	public float delay = 10f;
	float offsetX = 2.2f;
	float offsetY = - 0.3f;



	void Start () {
//		StartCoroutine (GenerateWave());
		
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.GetComponent<DefenderScript> () != null) {

			if (col.gameObject.GetComponent<TeamAssignment> ().myTeam == TeamAssignment.Team.TEAM_A) {
				ActivateGenerator (1, col.gameObject);
			} else if (col.gameObject.GetComponent<TeamAssignment> ().myTeam == TeamAssignment.Team.TEAM_B) {
				ActivateGenerator (2, col.gameObject);
			}

		}
	}

	void ActivateGenerator(int teamNum, GameObject player){
		player.transform.position = transform.position;
		Vector3 sourcePos1 = new Vector3 (source1.position.x - offsetX, source1.position.y + offsetY, source1.position.z); 
		Vector3 sourcePos2 = new Vector3 (source2.position.x + offsetX, source2.position.y + offsetY, source2.position.z);

		if (teamNum == 1) {
			teamAGeneratorActive = true;

			TeamManager.p1DefenderActive = false;
			TeamManager.p1CanSwitch = true;
			print ("Generator Triggered");
			StartCoroutine (GenerateWave (sourcePos1, sourcePos2, -1));

		} else {
			teamBGeneratorActive = true;
			TeamManager.p2DefenderActive = false;
			TeamManager.p2CanSwitch = true;
			print ("Generator Triggered");
			StartCoroutine (GenerateWave (sourcePos2, sourcePos1, 1));
		}
	}

	IEnumerator GenerateWave(Vector3 startPos, Vector3 targetPos, int mod){

		GameObject wave = Instantiate (prefab, startPos, Quaternion.identity);
		wave.GetComponent<WaveScript> ().targetPos = targetPos;
		wave.GetComponent<WaveScript> ().dirMod = mod;


		// yield return 0; - Waits a frame (write it for each frame you want to wait)

		yield return new WaitForSeconds (delay);
		StartCoroutine (GenerateWave(startPos, targetPos, -1));

		// while (button condition false){ yield return 0)

		//yield return StartCouroutine (other couroutine ) - waits for a couroutine to finish



	}
}
