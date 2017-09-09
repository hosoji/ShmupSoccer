using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {

	public GameObject prefab;

	public Transform source1, source2;

	public float healthDepletion;

	public Material p1Material, p2Material;

	public int playerNum =0;
	public static bool teamAGeneratorActive = false;
	public static bool teamBGeneratorActive = false;

	public float delay = 10f;
	float offsetX = 2.4f;
	float offsetY = 0.5f;




	void OnTriggerEnter(Collider col){
		if (col.gameObject.GetComponent<DefenderScript> () != null) {

			if (col.gameObject.GetComponent<TeamAssignment> ().myTeam == TeamAssignment.Team.TEAM_A) {
				ActivateGenerator (1, col.gameObject);
				playerNum = 1;
			} else if (col.gameObject.GetComponent<TeamAssignment> ().myTeam == TeamAssignment.Team.TEAM_B) {
				ActivateGenerator (2, col.gameObject);
				playerNum = 2;
			}

		}
	}

	void Update(){
		if (GetComponentInChildren<DefenderScript> () != null) {
			GetComponentInChildren<HealthScript> ().DecreaseHealth (GetComponentInChildren<PlayerStats> ().generatorDamage);
			
		} else {
			StopAllCoroutines ();
			teamAGeneratorActive = false;
			teamBGeneratorActive = false;

		}
		
	}

	void ActivateGenerator(int teamNum, GameObject player){
		
		Vector3 sourcePos1 = new Vector3 (source1.position.x - offsetX, source1.position.y + offsetY, source1.position.z); 
		Vector3 sourcePos2 = new Vector3 (source2.position.x + offsetX, source2.position.y + offsetY, source2.position.z);

		player.transform.SetParent (transform, true);
		player.GetComponent<Rigidbody> ().isKinematic = true;
		player.transform.localPosition = Vector3.zero;
		player.GetComponentInChildren<Canvas> ().gameObject.SetActive (false);




//
		if (teamNum == 1) {
			teamAGeneratorActive = true;

			TeamManager.p1DefenderActive = false;
			TeamManager.p1CanSwitch = true;
			print ("Generator Triggered");
			StartCoroutine (GenerateWave (sourcePos2, sourcePos1, 1, teamNum));

		} else {
			teamBGeneratorActive = true;
			TeamManager.p2DefenderActive = false;
			TeamManager.p2CanSwitch = true;
			print ("Generator Triggered");
			StartCoroutine (GenerateWave (sourcePos1, sourcePos2, -1, teamNum));
		}
	}

	IEnumerator GenerateWave(Vector3 pos, Vector3 targetPos, int mod, int pNum){

		float offsetX = 2f;
		Vector3 startPos = new Vector3 (pos.x + offsetX * mod, pos.y - 1f, pos.z);

		GameObject wave = Instantiate (prefab, startPos  , Quaternion.identity);

		if (pNum == 1) {
			SetMaterialInChildren (wave, p1Material);
			
		} else {
			SetMaterialInChildren (wave, p2Material);
		}

		wave.GetComponent<WaveScript> ().targetPos = targetPos;
		wave.GetComponent<WaveScript> ().dirMod = mod;


		WaveCollisionScript[] waveNums = wave.GetComponentsInChildren<WaveCollisionScript> ();

		for (int i = 0; i < waveNums.Length; i++) {
			waveNums [i].playerNum = pNum;
		}


		// yield return 0; - Waits a frame (write it for each frame you want to wait)

		yield return new WaitForSeconds (delay);
		StartCoroutine (GenerateWave(pos, targetPos, mod, pNum));

		// while (button condition false){ yield return 0)

		//yield return StartCouroutine (other couroutine ) - waits for a couroutine to finish



	}

	void SetMaterialInChildren(GameObject obj, Material mat){
		MeshRenderer[] rends = obj.GetComponentsInChildren<MeshRenderer> ();
		for (int i = 0; i < rends.Length; i++) {
			rends [i].material = mat;
		}
	}
}
