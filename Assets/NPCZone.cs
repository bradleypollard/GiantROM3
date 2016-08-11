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

	private float counter;

	void Update ()
	{
		if (npcAccepted && npcAccepted.transform.parent == null) {
			if (npcSpeaking) {
				progressBar.enabled = true;
				if (counter < npcSpeakingTime) {
					counter += Time.deltaTime;
					progressBar.value = counter / npcSpeakingTime;
				} else {
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
		if (collider.transform.name.Contains ("NPC")) {
			npcAccepted = collider.gameObject;
		} 
	}

	void OnTriggerExit (Collider collider)
	{
		if (collider.transform.name.Contains ("NPC")) {
			npcAccepted = null;
			GetComponent<Renderer> ().material.SetColor ("_Color", finishedColour);
			progressBar.enabled = false;
			counter = 0;
		}
	}

}
