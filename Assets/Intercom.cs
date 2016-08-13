using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Intercom : MonoBehaviour
{
	[Header("REFERENCES")]
	[SerializeField] TruckDelivery deliveryScript;

	List<int> playerID = new List<int>();
	public bool isComing;

	[Header("SETTINGS")]
	[SerializeField] float waitTime;

	void Update()
	{
		foreach (int id in playerID)
		{
			if (Input.GetButton ("A_P" + id) && isComing == false)
			{
				isComing = true;
				GetComponent<AudioSource> ().Play ();
				Invoke("CallHardDrive", waitTime);
			}
		}
	}

	void CallHardDrive()
	{
		deliveryScript.HardDriveDelivery ();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.GetComponent<PlayerMovement> ())
		{
			playerID.Add(collider.gameObject.GetComponent<PlayerMovement> ().playerIndex);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.GetComponent<PlayerMovement> ())
		{	
			playerID.Remove (collider.gameObject.GetComponent<PlayerMovement> ().playerIndex);
		}
	}
}