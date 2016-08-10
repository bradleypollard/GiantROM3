using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpotlightControlPanel : MonoBehaviour
{
	[SerializeField] List<GameObject> stageLights;
	public int currentLight = 0;


	void
	OnValidate()
	{
		//just so this isn't moved instead of the individual stage lights or control panel
		transform.position = new Vector3 (0, 0, 0);
	}


	void
	SwitchState()
	{
		stageLights [currentLight].SetActive (false);
		currentLight++;

		if (currentLight == stageLights.Count)
			currentLight = 0;
		
			
		stageLights [currentLight].SetActive (true);
		GetComponent<AudioSource> ().Play ();
	}


	void
	OnGUI()
	{
		if (GUI.Button (new Rect(2, 2, 300, 20), "Test Spotlight Control Panel"))
		{
			SwitchState ();
		}
	}
}