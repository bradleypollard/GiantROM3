using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FittingRoomTrigger : MonoBehaviour
{
	[Header("REFERENCES")]
	[SerializeField] DressingRoomDoor door;


	List<Collider> players = new List<Collider> ();


	void
	OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerMovement> () && door.occupied == false)
		{
			if (other.GetComponent<PlayerMovement>().isHoldingNPC == true)
			{
				players.Add (other);
				StartCoroutine (door.DoorOpen (true));
			}
		}
	}
		

	void
	OnTriggerExit(Collider other)
	{
		if(other.GetComponent<PlayerMovement>() && door.occupied == false)
		{
			players.Remove (other);
			if(players.Count == 0)
			{
				StartCoroutine(door.DoorOpen(false));
			}
		}
	}

}