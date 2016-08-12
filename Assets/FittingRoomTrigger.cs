using UnityEngine;
using System.Collections;

public class FittingRoomTrigger : MonoBehaviour
{
	[Header("REFERENCES")]
	[SerializeField] DressingRoomDoor door;

	[Header("Settings")]
	[SerializeField] bool exteriorTrigger;


	void
	OnTriggerEnter()
	{
		//if(player.HoldingNPC() == true)
		//{
		print("something");
		StartCoroutine (door.DoorOpen (true));

		//}
	}

	void
	OnTriggerExit()
	{
		StartCoroutine(door.DoorOpen(false));
	}

}