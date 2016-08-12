using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NPCZone : MonoBehaviour
{

	public GameObject npcAccepted;
	public bool npcSpeaking;

	[Range (1, 8)]
	public float npcSpeakingTime;

	public Slider progressBar;
	public Color notInUseColour;
	public Color inUseColour;
	public Color finishedColour;

	public bool requireWater;
	public float timeTilWater;
	public bool waitingWater;

	private float counter;

	void Update ()
	{
		if (npcAccepted && npcAccepted.transform.parent == null) {
			if (npcSpeaking) {
				progressBar.gameObject.SetActive (true);
				if (requireWater && !waitingWater && counter > timeTilWater) {
					waitingWater = true;
				} else if (counter < npcSpeakingTime && !waitingWater) {
					counter += Time.deltaTime;
					progressBar.value = counter / npcSpeakingTime;
				} else if (counter > npcSpeakingTime) {
					GetComponent<Renderer> ().material.SetColor ("_Color", finishedColour);
				}
			} else {
				npcSpeaking = true;
				GetComponent<Renderer> ().material.SetColor ("_Color", inUseColour);
			}
		}
	}

	void OnTriggerEnter (Collider collider)
	{
		Debug.Log (collider.name + " Entered");

		if (collider.transform.name.Contains ("NPC") && !npcAccepted) {
			npcAccepted = collider.gameObject;
			if (Random.Range (0, 10) > 5) {
				requireWater = true;
				timeTilWater = Random.Range (1, npcSpeakingTime);
			} else {
				requireWater = false;
				timeTilWater = 0;
			}
		} 

		if (waitingWater && collider.name.Contains ("Water")) {
			waitingWater = false;
			requireWater = false;
		}
	}

	void OnTriggerExit (Collider collider)
	{
		if (collider.transform.name.Contains ("NPC")) {
			npcAccepted = null;
			GetComponent<Renderer> ().material.SetColor ("_Color", notInUseColour);
			progressBar.gameObject.SetActive (false);
			counter = 0;
		}
	}

}
